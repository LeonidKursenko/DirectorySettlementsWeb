using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Repositories;
using DirectorySettlementsDAL.Interfaces;
using DirectorySettlementsBLL.Interfaces;
using DirectorySettlementsBLL.DTO;
using AutoMapper;
using DirectorySettlementsWebApi.Models;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using DirectorySettlementsBLL.BusinessModels;
using DirectorySettlementsBLL.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace DirectorySettlementsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementsController : ControllerBase
    {
        private readonly IDirectoryService _directoryService;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SettlementsController(ILogger<SettlementsController> logger, IDirectoryService directoryService, IExportService exportService)
        {
            _logger = logger;
            _directoryService = directoryService;
            _exportService = exportService;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<SettlementDTO, Node>()).CreateMapper();
        }

        // GET: api/Settlements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node>>> GetSettlements()
        {
            var settlementDTOs = await _directoryService.GetAllAsync();
            if(settlementDTOs == null || settlementDTOs.Any() == false)
            {
                return NotFound("Settlements not found.");
            }
            IEnumerable<Node> nodes = _mapper.Map<IEnumerable<SettlementDTO>, List<Node>>(settlementDTOs);
            return Ok(nodes.ToList());
        }

        // Get: api/settlements/filter?name=КИЇВ&type=Р
        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<IEnumerable<Node>>> GetFilteredSettlements(string name = null, string type = null)
        {
            var options = new FilterOptions
            {
                Name = name,
                SettlementType = type
            };
            var settlementDTOs = await _directoryService.FilterAsync(options);
            if (settlementDTOs == null || settlementDTOs.Any() == false)
            {
                string message = "Settlements not found.";
                _logger.LogWarning(message);
                return NotFound(message);
            }
            IEnumerable<Node> nodes = _mapper.Map<IEnumerable<SettlementDTO>, List<Node>>(settlementDTOs);
            return Ok(nodes.ToList());
        }

        // GET: api/Settlements/node?te=0110112001
        [HttpGet]
        [Route("node")]
        public async Task<ActionResult<Node>> GetSettlement(string te)
        {
            var settlementDTO = await _directoryService.GetAsync(te);
            if (settlementDTO == null)
            {
                string message = $"Failed to find a node with TE={te}.";
                _logger.LogWarning(message);
                return NotFound(message);
            }
            Node node = _mapper.Map<Node>(settlementDTO);
            return Ok(node);
        }

        // PUT: api/Settlements/0110112001
        [HttpPut("{te}")]
        public async Task<ActionResult<Node>> PutSettlement(string te, Node node)
        {
            if (node == null || te != node.Te)
            {
                string message = $"Failed to update operation.";
                _logger.LogWarning(message);
                return BadRequest(message);
            }
            SettlementDTO settlementDTO = new SettlementDTO
            {
                Te = node.Te,
                Nu = node.Nu,
                Np = node.Np
            };

            try
            {
                await _directoryService.UpdateAsync(settlementDTO);                
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
                //bool isExists = await SettlementExistsAsync(te);
                //if (!isExists)
                //{
                //    return NotFound($"Failed to update unexisted node with TE={te}.");
                //}
                //else
                //{
                //    throw;
                //}
            }
            //return NoContent();
            return Ok(node);
        }

        // POST: api/Settlements
        [HttpPost]
        public async Task<ActionResult<Node>> PostSettlement(Node node)
        {
            if (node == null)
            {
                string message = "Failed to create empty node.";
                _logger.LogWarning(message);
                return BadRequest(message);
            }
            SettlementDTO settlementDTO = new SettlementDTO
            {
                Te = node.Te,
                Nu = node.Nu,
                Np = node.Np
            };

            try
            {
                await _directoryService.CreateAsync(settlementDTO);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                bool isExists = await SettlementExistsAsync(settlementDTO.Te);
                if (isExists)
                {
                    return Conflict($"Failed to create node with existed TE={node.Te}.");
                }
                else
                {
                    return BadRequest($"Failed to create node with TE={node.Te}.");
                }
            }

            return Ok(node);
        }

        // DELETE: api/settlements/delete?te=0110112000&cascade=true
        [HttpDelete("delete")]
        public async Task<ActionResult<Node>> DeleteSettlement(string te, bool cascade)
        {
            SettlementDTO settlementDTO = await _directoryService.GetAsync(te);
            if (settlementDTO == null)
            {
                string message = $"Failed to delete unexisted node with TE={te}.";
                _logger.LogWarning(message);
                return NotFound(message);
            }
            try
            {
                await _directoryService.DeleteAsync(te, cascade);
            }
            catch(ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest($"Failed to delete node with TE={te}.");
            }

            Node node = _mapper.Map<Node>(settlementDTO);
            return Ok(node);
        }

        private async Task<bool> SettlementExistsAsync(string te)
        {
            var settlement = await _directoryService.GetAsync(te);
            return settlement != null;
        }

        // Get: api/settlements/export?name=КИЇВ&type=Р
        [HttpGet]
        [Route("export")]
        public async Task<IActionResult> GetExportSettlements(string name = null, string type = null)
        {
            var options = new FilterOptions
            {
                Name = name,
                SettlementType = type
            };
            var settlementDTOs = await _directoryService.FilterAsync(options);
            if (settlementDTOs == null || settlementDTOs.Any() == false)
            {
                return NotFound();
            }

            byte[] mas = _exportService.Export(settlementDTOs);
            string file_type = "application/pdf";
            string file_name = "document.pdf";
            return File(mas, file_type, file_name);
        }
    }
}

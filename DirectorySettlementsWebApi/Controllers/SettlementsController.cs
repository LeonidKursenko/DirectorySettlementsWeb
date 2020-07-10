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

namespace DirectorySettlementsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementsController : ControllerBase
    {
        private readonly IDirectoryService _directoryService;
        private readonly IMapper _mapper;

        public SettlementsController(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
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
                return NotFound();
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
                return NotFound($"Failed to find a node with TE={te}.");
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
                return BadRequest($"Failed to update operation.");
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
            catch (Exception ex)
            {
                bool isExists = await SettlementExistsAsync(te);
                if (!isExists)
                {
                    return NotFound($"Failed to update unexisted node with TE={te}.");
                }
                else
                {
                    throw;
                }
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
                return BadRequest("Failed to create empty node.");
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
            catch (Exception ex)
            {
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
                return NotFound($"Failed to delete unexisted node with TE={te}.");
            }
            try
            {
                await _directoryService.DeleteAsync(te, cascade);
            }
            catch(Exception ex)
            {
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
    }
}

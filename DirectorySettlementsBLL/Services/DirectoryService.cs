using AutoMapper;
using DirectorySettlementsBLL.BusinessModels;
using DirectorySettlementsBLL.DTO;
using DirectorySettlementsBLL.Exceptions;
using DirectorySettlementsBLL.Interfaces;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySettlementsBLL.Services
{
    /// <summary>
    /// DirectoryService provides CRUD operations with settlements tree.
    /// </summary>
    public class DirectoryService : IDirectoryService
    {
        public IUnitOfWork Manager { get; set; }

        private readonly IMapper _mapper;
        private readonly IFilter _filter;

        public DirectoryService(IUnitOfWork unitOfWork)
        {
            Manager = unitOfWork;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Settlement, SettlementDTO>()).CreateMapper();
            _filter = new Filter();
        }

        public async Task CreateAsync(SettlementDTO node)
        {
            Settlement settlement = new Settlement
            {
                Te = node.Te,
                Np = node.Np,
                Nu = node.Nu
            };
            try
            {
                await Manager.Settlements.CreateAsync(settlement);
            }
            catch(Exception ex)
            {
                throw new ValidationException($"Failed to create a settlement with TE={node.Te}", node.Te);
            }
        }

        public async Task DeleteAsync(string te, bool isCascade)
        {
            try
            {
                if (isCascade == true)
                {
                    await Manager.Settlements.DeleteAllAsync(te);
                }
                else
                {
                    await Manager.Settlements.DeleteAsync(te);
                    await Manager.SaveAsync();
                }
            }
            catch(Exception ex)
            {
                throw new ValidationException($"Failed to delete node with TE={te}." + ex.Message, te);
            }
        }

        public async Task<IEnumerable<SettlementDTO>> FilterAsync(IFilterOptions filterOptions)
        {
            IEnumerable<Settlement> settlements = await Manager.Settlements.GetAllAsync();
            settlements = settlements.Where(s => s.ParentId == null);
            IEnumerable<SettlementDTO> settlementDTOs = _mapper.Map<IEnumerable<Settlement>, List<SettlementDTO>>(settlements);
            if (settlementDTOs.Any() == false) return settlementDTOs;
            Filter(settlementDTOs, filterOptions);
            return settlementDTOs;
        }

        private void Filter(IEnumerable<SettlementDTO> settlementDTOs, IFilterOptions filterOptions)
        {
            foreach (var settlementDTO in settlementDTOs.ToList())
            {
                Filter(settlementDTO.Children, filterOptions);
                if (_filter.IsTrue(settlementDTO, filterOptions) == false)
                {
                    if(settlementDTO.Children.Any() == false)
                    {
                        (settlementDTOs as List<SettlementDTO>).Remove(settlementDTO);
                    }
                }
            }
        }

        public async Task<IEnumerable<SettlementDTO>> GetAllAsync()
        {
            
            IEnumerable<Settlement> settlements = await Manager.Settlements.GetAllAsync();
            IEnumerable<SettlementDTO> settlementDTOs = _mapper.Map<IEnumerable<Settlement>, List<SettlementDTO>>(settlements);
            return settlementDTOs.Where(s => s.ParentId == null);
        }

        public async Task<SettlementDTO> GetAsync(string te)
        {
            Settlement settlement = await Manager.Settlements.GetAsync(te);
            return _mapper.Map<SettlementDTO>(settlement);
        }

        public async Task<IEnumerable<SettlementDTO>> GetChildrenAsync(string parentTe)
        {
            IEnumerable<Settlement> settlements;
            if (parentTe == null)
            {
                settlements = await Manager.Settlements.GetAllAsync();
                settlements = settlements.Where(s => s.ParentId == null);
            }
            else
            {
                Settlement parentSettlement = await Manager.Settlements.GetAsync(parentTe);
                settlements = parentSettlement.Children;
            }
            IEnumerable<SettlementDTO> settlementDTOs = _mapper.Map<IEnumerable<Settlement>, List<SettlementDTO>>(settlements);
            return settlementDTOs;
        }

        public async Task UpdateAsync(SettlementDTO node)
        {
            Settlement settlement = await Manager.Settlements.GetAsync(node.Te);
            settlement.Nu = node.Nu;
            settlement.Np = node.Np;
            try
            {
                await Manager.Settlements.UpdateAsync(settlement);
            }
            catch(Exception ex)
            {
                throw new ValidationException($"Failed to update node with TE={node.Te}." + ex.Message, node.Te);
            }
        }

        public void Dispose()
        {
            Manager.Dispose();
        }
    }
}

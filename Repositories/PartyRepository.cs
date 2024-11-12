using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Dto;
using Gvz.Laboratory.ManufacturerService.Entities;
using Gvz.Laboratory.ManufacturerService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ManufacturerService.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly GvzLaboratoryManufacturerServiceDbContext _context;
        private readonly IManufacturerRepository _manufacturerRepository;

        public PartyRepository(GvzLaboratoryManufacturerServiceDbContext context, IManufacturerRepository manufacturerRepository = null)
        {
            _context = context;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Guid> CreatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id);

            if (existingParty == null)
            {
                var manufacturerEntity = await _manufacturerRepository.GetManufacturerEntityByIdAsync(party.ManufacturerId)
                    ?? throw new InvalidOperationException($"Manufacturer with Id '{party.ManufacturerId}' was not found.");

                var partyEntity = new PartyEntity
                {
                    Id = party.Id,
                    BatchNumber = party.BatchNumber,
                    DateOfReceipt = party.DateOfReceipt,
                    ProductName = party.ProductName,
                    SupplierName = party.SupplierName,
                    Manufacturer = manufacturerEntity,
                    BatchSize = party.BatchSize,
                    SampleSize = party.SampleSize,
                    TTN = party.TTN,
                    DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety,
                    TestReport = party.TestReport,
                    DateOfManufacture = party.DateOfManufacture,
                    ExpirationDate = party.ExpirationDate,
                    Packaging = party.Packaging,
                    Marking = party.Marking,
                    Result = party.Result,
                    Note = party.Note,
                    Surname = party.Surname,
                };

                await _context.Parties.AddAsync(partyEntity);
                await _context.SaveChangesAsync();
            }

            return party.Id;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> GetManufacturerPartiesForPageAsync(Guid manufacturerId, int pageNumber)
        {
            var partyEntities = await _context.Parties
                .AsNoTracking()
                .Where(p => p.Manufacturer.Id == manufacturerId)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberParties = await _context.Parties
                .Where(p => p.Manufacturer.Id == manufacturerId)
                .CountAsync();

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                p.ProductName,
                p.SupplierName,
                ManufacturerModel.Create(p.Manufacturer.Id, p.Manufacturer.ManufacturerName, false).manufacturer,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note
                )).ToList();

            return (parties, numberParties);
        }

        public async Task<Guid> UpdatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id)
                ?? throw new InvalidOperationException($"Party with Id '{party.Id}' was not found."); ;

            var manufacturerEntity = await _manufacturerRepository.GetManufacturerEntityByIdAsync(party.ManufacturerId)
                ?? throw new InvalidOperationException($"Manufacturer with Id '{party.ManufacturerId}' was not found.");

            existingParty.BatchNumber = party.BatchNumber;
            existingParty.DateOfReceipt = party.DateOfReceipt;
            existingParty.ProductName = party.ProductName;
            existingParty.SupplierName = party.SupplierName;
            existingParty.Manufacturer = manufacturerEntity;
            existingParty.BatchSize = party.BatchSize;
            existingParty.SampleSize = party.SampleSize;
            existingParty.TTN = party.TTN;
            existingParty.DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety;
            existingParty.TestReport = party.TestReport;
            existingParty.DateOfManufacture = party.DateOfManufacture;
            existingParty.ExpirationDate = party.ExpirationDate;
            existingParty.Packaging = party.Packaging;
            existingParty.Marking = party.Marking;
            existingParty.Result = party.Result;
            existingParty.Note = party.Note;

            await _context.SaveChangesAsync();

            return party.Id;
        }

        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}

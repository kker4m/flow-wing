using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowWing.DataAccess.Concrete;

public class AttachmentRepository : IAttachmentRepository
{
    
    private readonly FlowWingDbContext _dbContext;

    public AttachmentRepository(FlowWingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Attachment> CreateAttachmentAsync(Attachment attachment)
    {
        await _dbContext.Attachments.AddAsync(attachment);
        await _dbContext.SaveChangesAsync();
        return attachment;
    }

    public async Task<Attachment> UpdateAttachmentAsync(Attachment attachment)
    {
        _dbContext.Entry(attachment).State = EntityState.Modified;
        _dbContext.Attachments.Update(attachment);
        await _dbContext.SaveChangesAsync();
        return attachment;
    }

    public async Task<Attachment> DeleteAttachmentAsync(Attachment attachment)
    {
        _dbContext.Attachments.Remove(attachment);
        await _dbContext.SaveChangesAsync();
        return attachment;
    }

    public async Task<Attachment> GetAttachmentByIdAsync(int id)
    {
        return await _dbContext.Attachments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);;
    }

    public Attachment GetAttachmentById(int id)
    {
        return _dbContext.Attachments.AsNoTracking().FirstOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<Attachment>> GetAllAttachmentsAsync()
    {
        return await _dbContext.Attachments.AsNoTracking().ToListAsync();;
    }

    public async Task<IEnumerable<Attachment>> GetAttachmentsByEmailLogIdAsync(int emailLogId)
    {
        //get attachment id's from email log and return them
        var attachmentIds = _dbContext.EmailLogs.AsNoTracking().FirstOrDefault(x => x.Id == emailLogId)?.AttachmentIds;
        if (attachmentIds == null)
        {
            return new List<Attachment>();
        }
        var attachmentIdList = attachmentIds.Split(',').Select(int.Parse).ToList();
        return await _dbContext.Attachments.AsNoTracking().Where(x => attachmentIdList.Contains(x.Id)).ToListAsync();
    }
}
using FlowWing.Entities;

namespace FlowWing.Business.Abstract;

public interface IAttachmentService
{
    Task<Attachment> CreateAttachmentAsync(Attachment attachment);
    Task<Attachment> UpdateAttachmentAsync(Attachment attachment);
    Task<Attachment> DeleteAttachmentAsync(Attachment attachment);

    Task<Attachment> GetAttachmentByIdAsync(int id);
    Attachment GetAttachmentById(int id);
    Task<IEnumerable<Attachment>> GetAllAttachmentsAsync();
    Task<IEnumerable<Attachment>?> GetAttachmentsByEmailLogIdAsync(int emailLogId);
}
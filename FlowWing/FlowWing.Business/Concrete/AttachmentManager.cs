using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;

namespace FlowWing.Business.Concrete;

public class AttachmentManager : IAttachmentService
{
    private IAttachmentRepository _attachmentRepository;
    
    public AttachmentManager(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }
    
    public async Task<Attachment> CreateAttachmentAsync(Attachment attachment)
    {
        if (await _attachmentRepository.GetAttachmentByIdAsync(attachment.Id) != null)
        {
            throw new Exception("Attachment already exists");
        }
        else
        {
            await _attachmentRepository.CreateAttachmentAsync(attachment);
            return attachment;
        }
    }

    public async Task<Attachment> UpdateAttachmentAsync(Attachment attachment)
    {
        if (await _attachmentRepository.GetAttachmentByIdAsync(attachment.Id) == null)
        {
            throw new Exception("Attachment does not exist");
        }
        else
        {
            await _attachmentRepository.UpdateAttachmentAsync(attachment);
            return attachment;
        }
    }

    public async Task<Attachment> DeleteAttachmentAsync(Attachment attachment)
    {
        if (await _attachmentRepository.GetAttachmentByIdAsync(attachment.Id) == null)
        {
            throw new Exception("Attachment does not exist");
        }
        else
        {
            await _attachmentRepository.DeleteAttachmentAsync(attachment);
            return attachment;
        }
    }

    public async Task<Attachment> GetAttachmentByIdAsync(int id)
    {
        return await _attachmentRepository.GetAttachmentByIdAsync(id);
    }

    public Attachment GetAttachmentById(int id)
    {
        return _attachmentRepository.GetAttachmentById(id);
    }

    public async Task<IEnumerable<Attachment>> GetAllAttachmentsAsync()
    {
        return await _attachmentRepository.GetAllAttachmentsAsync();
    }

    public async Task<IEnumerable<Attachment>?> GetAttachmentsByEmailLogIdAsync(int emailLogId)
    {
        return await _attachmentRepository.GetAttachmentsByEmailLogIdAsync(emailLogId);
    }
}
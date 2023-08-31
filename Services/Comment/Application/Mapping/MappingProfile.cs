using Application.Requests;
using AutoMapper;

namespace Application.Mapping;

public class MappingProfile: Profile
{
    public MappingProfile()
    {			
        CreateMap<GrpcService1.CommentRequestDTO, UpdateCommentRequest>()
            .ForMember(updatecommentrequest => updatecommentrequest.id
                ,opt => opt.MapFrom(UpdateCommentRequest => UpdateCommentRequest.Id))
            .ForMember(updatecommentrequest => updatecommentrequest.Content
                ,opt => opt.MapFrom(UpdateCommentDTO => UpdateCommentDTO.Content));
        CreateMap<GrpcService1.DeleteCommentRequestDTO, DeleteCommentRequest>()
            .ForMember(deletecommentrequest => deletecommentrequest.id
                , opt => opt.MapFrom(deleteCommentRequest => deleteCommentRequest.Id));
        CreateMap<GrpcService1.CommentRequestDTO, CreateCommentRequest>()
            .ForMember(updatecommentrequest => updatecommentrequest.Postid
                ,opt => opt.MapFrom(createCommentdto => createCommentdto.Id))
            .ForMember(updatecommentrequest => updatecommentrequest.Content
                ,opt => opt.MapFrom(createCommentdto => createCommentdto.Content));
    }
}
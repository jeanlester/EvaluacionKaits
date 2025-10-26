using FluentValidation; 
using MediatR; 
namespace Kaits.Application.Behaviors; 
public class ValidationBehavior<TReq,TRes>:IPipelineBehavior<TReq,TRes> where TReq:IRequest<TRes>{ private readonly System.Collections.Generic.IEnumerable<IValidator<TReq>> _validators; public ValidationBehavior(System.Collections.Generic.IEnumerable<IValidator<TReq>> validators)=>_validators=validators; public async System.Threading.Tasks.Task<TRes> Handle(TReq request, RequestHandlerDelegate<TRes> next, System.Threading.CancellationToken ct){ if(_validators!=null){ var ctx=new ValidationContext<TReq>(request); 
            var fails=(await System.Threading.Tasks.Task.WhenAll(_validators.Select(v=>v.ValidateAsync(ctx,ct)))).SelectMany(r=>r.Errors).Where(f=>f!=null).ToList(); if(fails.Count!=0) throw new ValidationException(fails);} return await next(); 
    }
}
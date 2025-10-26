using FluentValidation; 
namespace Kaits.Application.Commands.CreateOrder; 
public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>{ public CreateOrderCommandValidator(){ RuleFor(x=>x.CustomerId).GreaterThan(0); 
        RuleFor(x=>x.Items).NotEmpty().WithMessage("Debe incluir al menos un producto."); 
        RuleForEach(x=>x.Items).ChildRules(items=>{ items.RuleFor(i=>i.ProductId).GreaterThan(0); 
            items.RuleFor(i=>i.Quantity).GreaterThan(0); items.RuleFor(i=>i.UnitPrice).GreaterThan(0); }); 
    } 
}
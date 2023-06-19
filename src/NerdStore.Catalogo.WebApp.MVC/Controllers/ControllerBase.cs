﻿using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.Catalogo.WebApp.MVC.Controllers;

public abstract class ControllerBase : Controller
{
    private readonly DomainNotificationHandler _notifications;
    private readonly IMediatorHandler _mediatorHandler;

    protected Guid ClienteId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");

    protected ControllerBase(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler,
        IHttpContextAccessor httpContextAccessor)
    {
        _notifications = (DomainNotificationHandler)notifications;
        _mediatorHandler = mediatorHandler;
        
        if(!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) return;

        var claim = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ClienteId = Guid.Parse(claim);
    }

    protected bool OperacaoValida()
    {
        return !_notifications.TemNotificacao();
    }

    protected IEnumerable<string> ObterMensagensErro()
    {
        return _notifications
            .ObterNotificacoes()
            .Select(c => c.Value)
            .ToList();
    }

    protected void NotificarErro(string codigo, string mensagem)
    {
        _mediatorHandler
            .PublicarNotificacao(new DomainNotification(codigo, mensagem));
    }
    
    protected new IActionResult Response(object result = null)
    {
        if (OperacaoValida())
        {
            return Ok(new
            {
                success = true,
                data = result
            });
        }

        return BadRequest(new
        {
            success = false,
            errors = _notifications.ObterNotificacoes().Select(n => n.Value)
        });
    }
}
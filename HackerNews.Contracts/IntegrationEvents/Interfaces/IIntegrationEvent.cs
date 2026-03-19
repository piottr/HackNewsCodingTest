using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNews.Contracts.IntegrationEvents.Interfaces;

public interface IIntegrationEvent
{
    Guid Id { get; }

    DateTime CreatedAt { get; }
}

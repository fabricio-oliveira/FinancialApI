﻿using System.Text;
using FinancialApi.Models.DTO.Request;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class EntryQueue :GenericQueue<PaymentDTO>
    {
        public EntryQueue(QueueContext context):base(context, context.PaymentQueueName) {}
    }
}
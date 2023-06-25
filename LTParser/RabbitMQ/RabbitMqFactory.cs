﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTParser.RabbitMQ
{
    public class RabbitMqFactory
    {
        private RabbitMqFactory() { }
        private static RabbitMqFactory instance;

        public static RabbitMqFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RabbitMqFactory();
                }
                return instance;
            }
        }
        public RabbitMqAbs create(string rabbitType,string exchange )
        {
            if( rabbitType == Constants.SENDER )
            {
                return RabbitMqSender.getInstance(exchange);
            }
            else if (rabbitType == Constants.RECIEVER)
            {
                return  RabbitMqReceiver.getInstance(exchange);
            }
            else
            {
                return null;
            }
            
        }
    }
}

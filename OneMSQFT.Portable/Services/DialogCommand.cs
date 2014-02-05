using System;

namespace OneMSQFT.Common.Services
{
    public class DialogCommand
    {
        public object Id { get; set; }
        public string Label { get; set; }
        public Action Invoked { get; set; }
    }
}
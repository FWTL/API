using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace FWTL.Common.Commands
{
    public class Response
    {
        public Response()
        {
        }

        public Response(Guid id)
        {
            Id = id;
        }

        public Response(ValidationException validationException)
        {
            Errors = validationException.Errors;
        }

        public Guid Id { get; set; }

        public IEnumerable<ValidationFailure> Errors { get; set; } = new List<ValidationFailure>();
    }

    public class Response<TResult> : Response
    {
        public Response(Guid id, TResult result) : base(id)
        {
            Result = result;
        }

        public TResult Result { get; set; }
    }
}
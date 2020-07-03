using AutoMapper;
using FWTL.Common.Extensions;
using FWTL.Core.Commands;
using System;
using System.Linq;

namespace FWTL.Common.Commands
{
    public class RequestToCommandProfile : Profile
    {
        public RequestToCommandProfile()
        {
        }

        public RequestToCommandProfile(Type type)
        {
            var classes = type.Assembly.GetTypes().Where(x => !x.IsNested).ToList();
            foreach (var @class in classes)
            {
                var nestedClasses = @class.GetNestedTypes();
                var command = nestedClasses.FirstOrDefault(t => typeof(ICommand).IsAssignableFrom(t));
                var request = nestedClasses.FirstOrDefault(t => command?.IsSubclassOf(t) ?? false);
                if (command.IsNotNull() && request.IsNotNull())
                {
                    CreateMap(request, command).ConstructUsingServiceLocator();
                }
            }
        }
    }
}
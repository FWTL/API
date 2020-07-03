using AutoMapper;
using FWTL.Common.Extensions;
using FWTL.Core.Queries;
using System;
using System.Linq;

namespace FWTL.Common.Queries
{
    public class RequestToQueryProfile : Profile
    {
        public RequestToQueryProfile()
        {
        }

        public RequestToQueryProfile(Type type)
        {
            var classes = type.Assembly.GetTypes().Where(x => !x.IsNested).ToList();
            foreach (var @class in classes)
            {
                var nestedClasses = @class.GetNestedTypes();
                var command = nestedClasses.FirstOrDefault(t => typeof(IQuery).IsAssignableFrom(t));
                var request = nestedClasses.FirstOrDefault(t => command?.IsSubclassOf(t) ?? false);
                if (command.IsNotNull() && request.IsNotNull())
                {
                    CreateMap(request, command).ConstructUsingServiceLocator();
                }
            }
        }
    }
}
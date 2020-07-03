using System;

namespace FWTL.Core.Events
{
    public class PropertyChanged : IEvent
    {
        public PropertyChanged(Guid aggregateId, string aggregateName, string typeName, string propertyName,
            object oldValue, object newValue) : this(aggregateId, aggregateName, typeName, propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public PropertyChanged(Guid aggregateId, string aggregateName, string typeName, string propertyName)
        {
            AggregateId = aggregateId;
            AggregateName = aggregateName;
            TypeName = typeName;
            PropertyName = propertyName;
        }

        public Guid AggregateId { get; set; }

        public string AggregateName { get; set; }

        public object NewValue { get; set; }

        public object OldValue { get; set; }

        public string PropertyName { get; set; }

        public string TypeName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sowalabs.Bison.ProfitSim
{
    public class SimulationPropertySetter
    {

        public class PropertyInfo
        {
            public string Name { get; }
            public Type Type { get; }

            public PropertyInfo(string name, Type type)
            {
                this.Name = name;
                this.Type = type;
            }
        }

        private readonly object _container;

        private readonly string _containerName;
        private List<PropertyInfo> _properties { get; }

        public SimulationPropertySetter(object propertiesContainer)
        {
            this._container = propertiesContainer;

            var type = propertiesContainer.GetType();

            this._containerName = type.FullName;

            this._properties = type.GetProperties().Where(property => property.CanWrite).Select(property => new PropertyInfo(property.Name, property.PropertyType)).ToList();
        }

        public void SetProperty(string name, object value)
        {
            this._container.GetType().GetProperties().First(property => property.Name == name).SetValue(this._container, value);
        }

    }
}

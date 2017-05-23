using Archetype.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Ozzy.Umbraco
{
    public static class ArchetypeExtensions
    {
        public static ArchetypeModel DeserializeJsonToArchetype(this string jsonString, string configString)
        {
            var archetype = JsonConvert.DeserializeObject<ArchetypeModel>(jsonString);
            var config = JsonConvert.DeserializeObject<ArchetypePreValue>(configString);

            foreach (var fieldset in config.Fieldsets)
            {
                foreach (var property in fieldset.Properties)
                {
                    property.PropertyEditorAlias = GetDataTypeByGuid(property.DataTypeGuid).PropertyEditorAlias;
                }
            }

            foreach (var fieldset in config.Fieldsets)
            {
                var fieldsetAlias = fieldset.Alias;
                foreach (var fieldsetInst in archetype.Fieldsets.Where(x => x.Alias == fieldsetAlias))
                {
                    foreach (var property in fieldset.Properties)
                    {
                        var propertyAlias = property.Alias;
                        foreach (var propertyInst in fieldsetInst.Properties.Where(x => x.Alias == propertyAlias))
                        {
                            propertyInst.DataTypeGuid = property.DataTypeGuid.ToString();
                            propertyInst.DataTypeId = GetDataTypeByGuid(property.DataTypeGuid).Id;
                            propertyInst.PropertyEditorAlias = property.PropertyEditorAlias;
                            propertyInst.HostContentType = null;
                        }
                    }
                }
            }
            return archetype;
        }

        private static IDataTypeDefinition GetDataTypeByGuid(Guid guid)
        {
            return (IDataTypeDefinition)ApplicationContext.Current.ApplicationCache.RuntimeCache.GetCacheItem(
                Archetype.Constants.CacheKey_DataTypeByGuid + guid,
                () => ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionById(guid));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace ItAintBoring.TestAssociatePlugin
{
    public class TestAssociatePlugin : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName == "Associate" || context.MessageName == "Disassociate")
{

                if (context.InputParameters.Contains("Relationship"))
                {

                    Relationship rel = (Relationship)context.InputParameters["Relationship"];

                    if (rel.SchemaName != "teammembership_association")
{
                        return;
                    }

                    if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                    {
                        var targetEntity = (EntityReference)context.InputParameters["Target"];
                        throw new InvalidPluginExecutionException(targetEntity.Name);
                        /*
                        if (context.InputParameters.Contains("RelatedEntities") && context.InputParameters["RelatedEntities"] is EntityReferenceCollection)
                        {

                            var relatedEntities = context.InputParameters["RelatedEntities"] as EntityReferenceCollection;
                            var relatedEntity = relatedEntities[0];
                        }
                        */
                        //check both relatedEntity and targetEntity – they might switch sides depending on what message is being handled and from where it came from

                    }
                }
            }

        }
    }
}

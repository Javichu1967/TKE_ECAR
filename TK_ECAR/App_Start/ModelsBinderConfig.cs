using System; 
using System.Web.Mvc; 
using TK_ECAR.Framework;
using TK_ECAR.Models;
 
using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR
{
    public class AlertaModelBinder : DefaultModelBinder
    {
        // this is the only method you need to override:
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (isIncludeBinder(modelType) )
            {

                var discriminator = bindingContext.ValueProvider.GetValue("IdTipoAlerta");

                int tipoAlerta = (int)discriminator.ConvertTo(typeof(int));

                Type instantiationType = getTypeInstance(modelType, tipoAlerta);

                var obj = Activator.CreateInstance(instantiationType);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
                bindingContext.ModelMetadata.Model = obj;
                return obj;
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        private bool isIncludeBinder(Type modelType)
        {
            var listaModelValidar = new List<Type> { typeof(AlertaModel), typeof(RenovarModel) };

            return listaModelValidar.Any(x => x == typeof(AlertaModel));
        }

        private Type getTypeInstance(Type modelType, int tipoAlerta)
        {
             
            if (modelType == typeof(AlertaModel))
                return getTypeInstanceAlertaModel(tipoAlerta);
            else
                return getTypeInstanceRenovacionModel(tipoAlerta);
        }
        private Type getTypeInstanceAlertaModel(int tipoAlerta)
        {

            switch (tipoAlerta)
            {
                case (int)EnumTipoAlerta.Multa:
                    return typeof(DatosMultaModel);
                case (int)EnumTipoAlerta.TarjetaSOLRED:
                    return typeof(DatosSOLREDModel);
                case (int)EnumTipoAlerta.Otras:
                    return typeof(DatosOtraNotificacionModel);
                case (int)EnumTipoAlerta.Robos:
                    return typeof(DatosRoboModel);
                case (int)EnumTipoAlerta.CambioConductor:
                    return typeof(DatosCambioConductorModel);
                default:
                    throw new InvalidOperationException("No se ha podido evaluar el tipo de alerta.");
            }

        }
        private Type getTypeInstanceRenovacionModel(int tipoAlerta)
        {

            switch (tipoAlerta)
            {
                case (int)EnumTipoAlerta.ITV:
                    return typeof(RenovarITVModel);
                case (int)EnumTipoAlerta.Carnet:
                    return typeof(RenovarCarnetModel);
                
                default:
                    throw new InvalidOperationException("No se ha podido evaluar el tipo de renovación alerta.");
            }

        }
    }
}

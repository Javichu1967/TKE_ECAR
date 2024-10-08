using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_PREGUNTAS_FRECUENTES : RepositoryBase<T_G_PREGUNTAS_FRECUENTES>, IRepositoryT_G_PREGUNTAS_FRECUENTES
    {
		public RepositoryT_G_PREGUNTAS_FRECUENTES(ModelEntities context)
            : base(context)
        {

        }
    }
}
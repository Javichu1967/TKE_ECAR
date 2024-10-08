using System;
using System.Data.Entity;
using System.Linq;


namespace TK_ECAR.Domain
{
    public partial class ModelEntities
    {
        public override int SaveChanges()
        {
            AuditarAlerta();

            return base.SaveChanges();
        }

        public void AuditarAlerta()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is T_G_ALERTAS
              && (x.State == EntityState.Added || x.State == EntityState.Modified));


            var fecha = DateTime.Now.Date;

            foreach (var entry in modifiedEntries)
            {

                T_G_ALERTAS entity = entry.Entity as T_G_ALERTAS;

                if (entity != null)
                {
                    if (entry.State == EntityState.Added)

                        entity.FECHA_CREACION = fecha;
                    else
                        entity.FECHA_MODIFICACION = fecha;


                    var historico = new T_G_HIST_ALERTAS
                    {
                        USUARIO_CREACION = entry.State == EntityState.Added ? 
                                                entity.USUARIO_CREACION : 
                                                entity.USUARIO_MODIFICACION,
                        ID_ESTADO = entity.ID_ESTADO,
                        ID_ACCION = entity.ID_ACCION,
                        FECHA_CREACION = fecha

                     };
                     

                    entity.T_G_HIST_ALERTAS.Add(historico);
                }

                
            }
        }
    }
}

using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{
    [ServiceContract]
    public interface ImarcacionService
    {
        [OperationContract]
        DatoRetorno<JQGrid> SelectPaginated_marcacion(Parametro dato);
        [OperationContract]
        DatoRetorno<bool> Insert_marcacion(marcacion dato);
        [OperationContract]
        DatoRetorno<bool> Delete_marcacion(marcacion dato);
        [OperationContract]
        DatoRetorno<DataTable> SelectAll_reporte_horas_trabajadas_toExport(Parametro dato);
    }
}

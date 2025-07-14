using System.Collections.Generic;
using System.ServiceModel;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{
    [ServiceContract]
    public interface ItrabajadorService
    {
        [OperationContract]
        DatoRetorno<JQGrid> SelectPaginated_trabajador(Parametro dato);
        [OperationContract]
        DatoRetorno<trabajador> Select_trabajador(trabajador dato);
        [OperationContract]
        DatoRetorno<trabajador> Select_trabajador_by_dni(trabajador dato);
        [OperationContract]
        DatoRetorno<bool> Insert_trabajador(trabajador dato);
        [OperationContract]
        DatoRetorno<bool> Update_trabajador(trabajador dato);
        [OperationContract]
        DatoRetorno<bool> Update_trabajador_captura(trabajador dato);
        [OperationContract]
        DatoRetorno<bool> Delete_trabajador(trabajador dato);

    }
}

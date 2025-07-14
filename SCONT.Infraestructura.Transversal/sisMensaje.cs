using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Infraestructura.Transversal
{
    public static class sisMensaje
    {
        public const string Insert_Ok = "El registro fue insertado";
        public const string Insert_Error = "El registro no pudo ser insertado, verifique";
        public const string Update_Ok = "El registro fue actualizado";
        public const string Update_Error = "El registro no pudo ser actualizado, verifique";
        public const string Delete_Ok = "El registro fue borrado";
        public const string Delete_Error = "El registro no pudo ser borrado, verifique";
        public const string Select_Error = "El registro solicitado no existe, verifique";
        public const string SelectAll_Error = "Los registros solicitados no existen, verifique";
        public const string SelectPaginated_Error = "No existen registros para la consulta efectuada, verifique";
        public const string Inactivate_Ok = "El registro fue inactivado";
        public const string Inactivate_Error = "El registro no pudo ser inactivado, verifique";
        public const string Importados_Ok= "Registros importados con éxito";
        public const string Cerrar_Ok = "Cierre del registro concluido";
        public const string Cerrar_Error = "El registro no pudo ser cerrado, verifique";


        public const string Name_Exist = "Ya existe un registro con el mismo nombre, verifique";
        public const string Register_NotExist = "El registro no existe, verifique";
        public const string Register_Inactivate = "El registro ya ha sido inactivado, verifique";
        public const string Register_Deleted = "El registro ya ha sido borrado, verifique";
        public const string Register_Closed = "El registro ya ha sido cerrado, verifique";

        public const string Notificar_Error = "Se ha producido un error al notificar";
        public const string Notificar_Ok = "Se han realizado las notificaciones con éxito";

        public const string Send_Ok = "El registro fue enviado con éxito";
        public const string Send_Error = "El registro no pudo ser enviado, verifique";
        public const string Approve_Ok = "El registro fue aprobado con éxito";
        public const string Approve_Error = "El registro no pudo ser aprobado, verifique";
        public const string Disapprove_Ok = "El registro fue rechazado con éxito";
        public const string Disapprove_Error = "El registro no pudo ser rechazado, verifique";
        public const string Refuse_Ok = " Se rechazo el requerimiento con éxito";
        public const string Refuse_Error = "No se logro rechazar el requerimiento, verifique";
        public const string Assing_Ok = " Se asigno el especialista de logistica con éxito";
        public const string Assing_Error = "No se logro asignar al especialista, verifique";

        public const string Approve_inf_Ok = "El informe fue aprobado con éxito";
        public const string Approve_inf_Error = "El informe no pudo ser aprobado, verifique";
        public const string Disapprove_inf_Ok = "El informe fue rechazado con éxito";
        public const string Disapprove_inf_Error = "El informe no pudo ser rechazado, verifique";



    }
}

/// <reference path="../../utils/ProyectoMED.js" />

$(document).ready(function () {
    //-----------inicial-------------
    iniProcess = false;
    PROYECTOMED.ConfigTab($("[name=idmenuset]").val(), $("[name=idopcset]").val());
    $("[id$=txtFiNUMERO_DOCUMENTO]").SoloNumerosEnteros();
    $("[id$=txtFilFECHA_DESDE]").SoloFecha();
    $("[id$=txtFilFECHA_HASTA]").SoloFecha();

    PROYECTOMED.SetFormatPicker("dtpFilFECHA_DESDE");
    PROYECTOMED.SetFormatPicker("dtpFilFECHA_HASTA");

    $("[id$=btnRegLimpiar]").click(function () {
        Registro.CleanRegistro();
        return false;
    });

    //-----------eventos-------------
    $("[id$=btnRepAceptar]").click(function () {
        Registro.GeneraReporte();
        return false;
    });

    $("[id$=ddlFilTID_TIPO_REPORTE]").change(function () {
        Registro.ShowFiltro();
        return false;
    });

    //-----------control-------------
    var Registro = {
        Edicion: false,
        CadenaUsuarios: "",
        //
        ShowFiltro: function () {
        },
        //
        CleanRegistro: function () {
            $("[id*='txtFil']").val('');
            $("[id*='ddlFil']").val('');
            $(".form-group").removeClass("has-error");
        },
        //
        CheckRegistro: function () {
            var tipo = $("[id$=ddlFilTID_TIPO_REPORTE]").val();
            if (!PROYECTOMED.CheckLength("ddlFilTID_TIPO_REPORTE", "Debe seleccionar un TIPO DE REPORTE", 1))
                return false;
          
            if (!PROYECTOMED.CheckLength("txtFilFECHA_DESDE", "Debe ingresar un valor en el campo FECHA PAGO DESDE", 1))
                return false;

            if (!PROYECTOMED.isValidDate("txtFilFECHA_DESDE", "Debe ingresar un valor correcto en el campo FECHA PAGO DESDE (dd/mm/yyyy)"))
                return false;

            if (!PROYECTOMED.CheckLength("txtFilFECHA_HASTA", "Debe ingresar un valor en el campo FECHA PAGO HASTA", 1))
                return false;

            if (!PROYECTOMED.isValidDate("txtFilFECHA_HASTA", "Debe ingresar un valor correcto en el campo FECHA PAGO HASTA (dd/mm/yyyy)"))
                return false;          

            return true;
        },
        //
        GeneraReporte: function () {
            if (!Registro.CheckRegistro())
                return false;

            var datParam = new Object();
            datParam.Parametro1 = $("[id$=ddlFilTID_TIPO_REPORTE]").val();
            datParam.Parametro2 = $("[id$=txtFiNUMERO_DOCUMENTO]").val();
            datParam.FECHA_DESDE = $("[id$=txtFilFECHA_DESDE]").val();
            datParam.FECHA_HASTA = $("[id$=txtFilFECHA_HASTA]").val();
            //
            datParam.Parametro10 = $("[id$=txtFilANIO]").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GenerarReporte"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        window.open(PROYECTOMED.UrlActionMethod("reporte", datTabla), '_blank');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                },
            });

        },
        //
    }

    //-----------grillas-------------
    
    iniProcess = true;
});

/// <reference path="../../utils/ProyectoMED.js" />

$(document).ready(function () {
    //-----------inicial-------------
    PROYECTOMED.ConfigTab($("[name=idmenuset]").val(), $("[name=idopcset]").val());
    $("[id*=txtFil]").prop("disabled", false);
    iniProcess = false;
    var dialogRegUsuario = $('#dialogRegUsuario');
    dialogRegUsuario.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) { });

    $("[id$=txtFilDNI]").SoloNumerosEnteros();
    $("[id$=txtFilAPELLIDO_PATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilAPELLIDO_PATERNO");
    $("[id$=txtFilAPELLIDO_MATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilAPELLIDO_MATERNO");
    $("[id$=txtUsuDNI]").SoloNumerosEnteros();
    $("[id$=txtUsuNOMBRES]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtUsuNOMBRES");
    $("[id$=txtUsuAPELLIDO_PATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtUsuAPELLIDO_PATERNO");
    $("[id$=txtUsuAPELLIDO_MATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtUsuAPELLIDO_MATERNO");
    $("[id$=txtUsuUSUARIO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtUsuUSUARIO");
    $("[id$=txtSigUSUARIO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtSigUSUARIO");
    $("[id$=txtSigNOMBRES]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtSigNOMBRES");
    $("[id$=txtUsuCORREO]").SoloEmail(); PROYECTOMED.ToUpperCaseSinEspaciosDoblesEmail("txtUsuCORREO");
    $("[id$=txtCCANO_EJE]").SoloNumerosEnteros();
    $("[id$=txtUsuCONTRASENIA]").SoloPassword(); PROYECTOMED.ToPasswordSinEspaciosDobles("txtUsuCONTRASENIA");
    $("[id$=txtUsuRUC]").SoloNumerosEnteros();

    $("[id$=txtUsuUSUARIO_SIGA]").InputDisabled();

    //-----------eventos-------------
    $("[id$=btnManConsulta]").click(function () {
        $('#tblData').trigger('reloadGrid');
        return false;
    });

    $("[id$=btnManLimpiar]").click(function () {
        Registro.CleanFiltro();
    });

    $("[id$=btnManNuevo]").click(function () {
        Registro.NewUsuario();
        return false;
    });

    $("[id$=btnUsuAceptar]").click(function () {
        Registro.UpdUsuario();
        return false;
    });

    $("[id$=btnBusDNI]").click(function () {
        Registro.GetPersona();
        return false;
    });

    $('#tblData').delegate('.btnUsuEdit', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnUsuEdit", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.GetUsuario(ret);
        return false;
    });

    $('#tblData').delegate('.btnUsuDel', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnUsuDel", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.DelUsuario(ret);
        return false;
    });

    $("[id$=btnVerPass]").click(function () {
        var x = document.getElementById("txtUsuCONTRASENIA");
        if (x.type === "password") {
            x.type = "text";
            $("#btnVerPass i").removeClass("fa-eye");
            $("#btnVerPass i").addClass("fa-eye-slash");
        } else {
            x.type = "password";
            $("#btnVerPass i").addClass("fa-eye");
            $("#btnVerPass i").removeClass("fa-eye-slash");
        }
        return false;
    });

    $("[id$=btnManRepLogistica]").click(function () {
        Registro.GeneraReporte(1);
        return false;
    });

    $("[id$=btnManRepOficina]").click(function () {
        Registro.GeneraReporte(2);
        return false;
    });

    //-----------funciones-------------
    var Registro = {
        CodigoUsuario: 0,
        Edicion: false,
        //
        CleanFiltro: function () {
            $("[id*='txtFil']").val('');
            $("[id*='ddlFil']").val('');
            $("[id$='txtFilANIO_EJE']").val($("[id$='hfAnioActual']").val());
            $('#tblData').jqGrid("clearGridData");
        },
        //
        CleanUsuario: function () {
            $("[id*='hfUsu']").val('');
            $("[id*='txtUsu']").val('');
            $("[id*='ddlUsu']").val('');
            Registro.Bloquear(false);
            $(".form-group").removeClass("has-error");
            var x = document.getElementById("txtUsuCONTRASENIA");
            x.type = "password";
            $("#btnVerPass i").addClass("fa-eye");
            $("#btnVerPass i").removeClass("fa-eye-slash");
            $("#registro_usuario").css("display", "block");
        },
        //
        Bloquear: function (bloquear) {
            $("[id*=txtUsu]").prop("disabled", bloquear);
            $("[id*=ddlUsu]").prop("disabled", bloquear);
        },
        //
        NewUsuario: function () {
            Registro.CleanUsuario();
            Registro.CodigoUsuario = 0;
            Registro.Edicion = false;
            dialogRegUsuario.modal("show");
        },
        //
        CheckUsuario: function () {
            if (!PROYECTOMED.CheckLength('txtUsuDNI', 'Debe ingresar un valor en el campo DNI', 1))
                return false;

            if ($("[id$=txtUsuDNI]").val() != "") {
                if (!PROYECTOMED.CheckLength('txtUsuDNI', 'El campo DNI debe tener como mínimo 8 dígitos', 8, 8))
                    return false;
            }
            
            if (!PROYECTOMED.CheckLength('txtUsuNOMBRES', 'Debe ingresar un valor en el campo NOMBRES', 1))
                return false;
            var apPat = $("[id$=txtUsuAPELLIDO_PATERNO]").val();
            var apMat = $("[id$=txtUsuAPELLIDO_MATERNO]").val();
            if (apPat.trim() == "" && apMat.trim() == "") {
                $("[id$=txtUsuAPELLIDO_PATERNO]").addClass("ui-state-error");
                $("[id$=txtUsuAPELLIDO_MATERNO]").addClass("ui-state-error");
                PROYECTOMED.MostrarMensaje('Debe ingresar un valor en el campo APELLIDO PATERNO o APELLIDO MATERNO', 'Error');
                $("[id$=txtUsuAPELLIDO_PATERNO]").focus();
                return false;
            } else {
                $("[id$=txtUsuAPELLIDO_PATERNO]").removeClass("ui-state-error");
                $("[id$=txtUsuAPELLIDO_MATERNO]").removeClass("ui-state-error");
            }

            if (!PROYECTOMED.CheckLength('ddlUsuID_ROL', 'Debe seleccionar un valor en el campo ROL', 1))
                return false;
            var rol = $("[id$=ddlUsuID_ROL]").val();
          
            if (!PROYECTOMED.CheckLength('txtUsuUSUARIO', 'Debe ingresar un valor en el campo USUARIO', 1))
                return false;
            if (!PROYECTOMED.CheckLength('txtUsuCONTRASENIA', 'Debe ingresar un valor en el campo CONTRASEÑA', 1))
                return false;
            if (!PROYECTOMED.CheckLength('txtUsuCONTRASENIA', 'El campo CONTRASEÑA debe tener como mínimo 8 caracteres', 8))
                return false;
         
            //if (!PROYECTOMED.CheckLength('txtUsuCORREO', 'Debe ingresar un valor en el campo CORREO ELECTRÓNICO', 1))
            //    return false;
            if ($("[id$=txtUsuCORREO]").val() != "") {
                if (!PROYECTOMED.CheckEmail('txtUsuCORREO', 'Debe ingresar un valor correcto en el campo CORREO ELECTRÓNICO'))
                    return false;
            }
            return true;

        },
        //
        GetUsuario: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_USUARIO = ret.cod;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetUsuario"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        Registro.CleanUsuario();
                        Registro.CodigoUsuario = datTabla.ID_USUARIO;
                        Registro.Edicion = true;
                        $("[id$=txtUsuUSUARIO]").val(datTabla.USUARIO);
                        $("[id$=txtUsuCONTRASENIA]").val(datTabla.CONTRASENIA);
                        $("[id$=txtUsuDNI]").val(datTabla.DNI);
                        $("[id$=txtUsuNOMBRES]").val(datTabla.NOMBRES);
                        $("[id$=txtUsuAPELLIDO_PATERNO]").val(datTabla.APELLIDO_PATERNO);
                        $("[id$=txtUsuAPELLIDO_MATERNO]").val(datTabla.APELLIDO_MATERNO);
                        $("[id$=ddlUsuID_ROL]").val(datTabla.ID_ROL);
                        $("[id$=txtUsuCORREO]").val(datTabla.CORREO);
                        $("[id$=txtUsuUSUARIO]").prop("disabled", true);
                       
                        dialogRegUsuario.modal("show");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },       
        //
        UpdUsuario: function () {
            if (!Registro.CheckUsuario())
                return false;

            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_USUARIO = Registro.CodigoUsuario;
            datParam.USUARIO = $("[id$=txtUsuUSUARIO]").val();
            datParam.CONTRASENIA = $("[id$=txtUsuCONTRASENIA]").val();
            datParam.DNI = $("[id$=txtUsuDNI]").val();
            datParam.NOMBRES = $("[id$=txtUsuNOMBRES]").val();
            datParam.APELLIDO_PATERNO = $("[id$=txtUsuAPELLIDO_PATERNO]").val();
            datParam.APELLIDO_MATERNO = $("[id$=txtUsuAPELLIDO_MATERNO]").val();
            datParam.ID_ROL = $("[id$=ddlUsuID_ROL]").val();
            datParam.CORREO = $("[id$=txtUsuCORREO]").val();

            PROYECTOMED.Ajax({
                url: PROYECTOMED.UrlActionMethod((Registro.Edicion ? "Upd" : "Ins") + "Usuario"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                tokenRequired: true,
                success: function (result) {                    
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#tblData").trigger("reloadGrid");
                        dialogRegUsuario.modal("hide");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });

        },
        //
        DelUsuario: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_USUARIO = ret.cod;

            PROYECTOMED.MostrarPopup('Borrar', '¿Seguro de borrar el registro?', 'SiNo', function () {
                
                PROYECTOMED.Ajax({
                    url: PROYECTOMED.UrlActionMethod("DelUsuario"),
                    data: PROYECTOMED.SetParam({ 'dato': datParam }),
                    tokenRequired: true,
                    success: function (result) {

                        var datTabla = result.Dato;
                        var msgTabla = result.Msg;
                        if (datTabla) {
                            $("#tblData").trigger("reloadGrid");
                            if (msgTabla)
                                PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                        } else {
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                        }

                    }
                });
            });
        },       
        //
        GeneraReporte: function (tipo) {
           
            var datParam = new Object();
            datParam.Parametro1 = tipo;

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
    }
    //-----------grillas-------------
    $("#tblData").jqGrid({
        datatype: function () {
            var parFiltro = PROYECTOMED.GetPaginationJQGrid('tblData');
            parFiltro.PARAMETRO1 = $("#txtFilDNI").val();
            parFiltro.PARAMETRO2 = $("#txtFilAPELLIDO_PATERNO").val();
            parFiltro.PARAMETRO4 = $("#txtFilAPELLIDO_MATERNO").val();
            parFiltro.PARAMETRO3 = $("#ddlFilID_ROL").val();
            PROYECTOMED.CargarGridLoad('GetBandeja', 'tblData', { 'dato': parFiltro });

        },
        colNames: ['ID_USUARIO', 'USUARIO',  'DNI', 'NOMBRES', 'APELLIDO PATERNO', 'APELLIDO MATERNO', 'ROL', 'CORREO', 'ACCIONES'],
        colModel: [
            { name: 'cod', index: 'cod', align: 'left', width: 5, sortable: false, hidden: true },
            { name: 'c01', index: 'c01', align: 'left', width: 15, sortable: false, hidden: true },
            { name: 'c02', index: 'c02', align: 'left', width: 15, sortable: false, hidden: false },
            { name: 'c04', index: 'c04', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c05', index: 'c05', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c06', index: 'c06', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c07', index: 'c07', align: 'left', width: 15, sortable: false, hidden: false },
            { name: 'c08', index: 'c08', align: 'left', width: 15, sortable: false, hidden: false },
            { name: 'c09', index: 'c09', align: 'center', width: 12, sortable: false, hidden: false, formatter: VerAccionesUsuario },
        ],
        rowNum: 15,
        rowList: [15, 30, 40],
        pager: '#pagData',
        sortname: 'c03',
        sortorder: 'asc',
        height: 'auto',
        scrollOffset: 0,
        autowidth: true,
        shrinkToFit: true,
        rownumbers: true,
        reloadAfterEdit: false,
        reloadAfterSubmit: false,
        viewrecords: true,
        toolbar: [false, 'top'],
        hidegrid: false,
        autoencode: true,
        caption: '',
        gridComplete: function () {
            $('#tblData tr td').css("white-space", "normal");
            $('.tooltip-demo').tooltip({
                selector: "[data-toggle=tooltip]",
                container: "body"
            });
        },
    }).navGrid('#pagData', { refresh: false, search: false, add: false, edit: false, del: false });

    $("#contData").resize(function () {
        $("#tblData").jqGrid('setGridWidth', $("#contData").width());
    });
    
    $(window).resize(function () {
        $('#tblData').jqGrid('setGridWidth', $('#contData').width());
    });

    function VerAccionesUsuario(cellvalue, options, rowObject) {
        var html = "";
        html = "<div class='tooltip-demo'>";
        html = html + "<button class='btnUsuEdit btn btn-in-grid' type='button'  id='btnUsuEdit" + options.rowId + "' data-original-title='Editar usuario' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-pencil' ></i></button>";
        html = html + "<button class='btnUsuDel btn btn-in-grid' type='button'  id='btnUsuDel" + options.rowId + "' data-original-title='Eliminar usuario' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-trash-o' ></i></button>";
        html = html + "</div>";
        return html;
    }

    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-x', 'hidden');
    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-y', 'hidden');

    iniProcess = true;
  
});

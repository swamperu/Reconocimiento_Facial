/// <reference path="../../utils/ProyectoMED.js" />

$(document).ready(function () {
    //-----------inicial-------------
    PROYECTOMED.ConfigTab($("[name=idmenuset]").val(), $("[name=idopcset]").val());
    $("[id*=txtFil]").prop("disabled", false);
    iniProcess = false;
    var dialogRegistro = $('#dialogRegistro');
    dialogRegistro.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) {
        PROYECTOMED.SetFormatPicker("dtpRegFECHA_NACIMIENTO");
        PROYECTOMED.SetFormatPicker("dtpRegFECHA_INGRESO");
    });

    var dialogLog = $('#dialogLog');
    dialogLog.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) { });



    $("[id$=txtFilDNI]").SoloNumerosEnteros();
    $("[id$=txtFilAPELLIDO_PATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilAPELLIDO_PATERNO");
    $("[id$=txtFilAPELLIDO_MATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtFilAPELLIDO_MATERNO");

    $("[id$=txtRegFECHA_NACIMIENTO]").SoloFecha();
    $("[id$=txtRegFECHA_INGRESO]").SoloFecha();
    $("[id$=txtRegDNI]").SoloNumerosEnteros();
    $("[id$=txtRegNOMBRES]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtRegNOMBRES");
    $("[id$=txtRegAPELLIDO_PATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtRegAPELLIDO_PATERNO");
    $("[id$=txtRegAPELLIDO_MATERNO]").SoloAlfaNumerico(true); PROYECTOMED.ToUpperCaseSinEspaciosDobles("txtRegAPELLIDO_MATERNO");
    $("[id$=txtRegCORREO]").SoloEmail(); PROYECTOMED.ToUpperCaseSinEspaciosDoblesEmail("txtRegCORREO");
    $("[id$=txtRegTELEFONO]").SoloNumerosEnteros();


    //-----------eventos-------------
    $("[id$=btnManConsulta]").click(function () {
        $('#tblData').trigger('reloadGrid');
        return false;
    });

    $("[id$=btnManLimpiar]").click(function () {
        Registro.CleanFiltro();
    });

    $("[id$=btnManNuevo]").click(function () {
        Registro.NewRegistro();
        return false;
    });

    $("[id$=btnRegAceptar]").click(function () {
        Registro.UpdRegistro();
        return false;
    });

    $('#tblData').delegate('.btnRegEdit', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnRegEdit", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.GetRegistro(ret);
        return false;
    });

    $('#tblData').delegate('.btnRegCap', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnRegCap", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.InitCaptura(ret);
        return false;
    });

    $('#tblData').delegate('.btnRegDel', 'click', function () {
        var id = $(this).attr("id");
        var id = id.replace("btnRegDel", "");
        $('#tblData').jqGrid('setSelection', parseInt(id), false);
        var ret = $("#tblData").getRowData(id);
        Registro.DelRegistro(ret);
        return false;
    });

    $("[id$=ddlRegDEPARTAMENTO]").change(function () {
        PROYECTOMED.ClearCombo("ddlRegPROVINCIA", "(SELECCIONAR)");
        PROYECTOMED.ClearCombo("ddlRegDISTRITO", "(SELECCIONAR)");
        var datParam = new Object();
        datParam.DEP_CODIGO = $("[id$=ddlRegDEPARTAMENTO]").val();
        PROYECTOMED.CargarCombo(PROYECTOMED.UrlActionMethod("trabajador", "GetProvincias"), "ddlRegPROVINCIA", { 'dato': datParam }, "(SELECCIONAR)", function () { });
        return false;
    });

    $("[id$=ddlRegPROVINCIA]").change(function () {
        PROYECTOMED.ClearCombo("ddlRegDISTRITO", "(SELECCIONAR)");
        var datParam = new Object();
        datParam.DEP_CODIGO = $("[id$=ddlRegDEPARTAMENTO]").val();
        datParam.PRV_CODIGO = $("[id$=ddlRegPROVINCIA]").val();
        PROYECTOMED.CargarCombo(PROYECTOMED.UrlActionMethod("trabajador", "GetDistritos"), "ddlRegDISTRITO", { 'dato': datParam }, "(SELECCIONAR)", function () { });
        return false;
    });

    //-----------funciones-------------
    let intervalo;

    var Registro = {
        Codigo: 0,
        Edicion: false,
        IdCaptura: 0,
        DniCaptura: "",
        //
        CleanFiltro: function () {
            $("[id*='txtFil']").val('');
            $("[id*='ddlFil']").val('');
            $('#tblData').jqGrid("clearGridData");
        },
        //
        CleanRegistro: function () {
            $("[id*='hfReg']").val('');
            $("[id*='txtReg']").val('');
            $("[id*='ddlReg']").val('');
            PROYECTOMED.ClearCombo("ddlRegPROVINCIA", "(SELECCIONAR)");
            PROYECTOMED.ClearCombo("ddlRegDISTRITO", "(SELECCIONAR)");
            $(".form-group").removeClass("has-error");
        },
        //
        Bloquear: function (bloquear) {
            $("[id*=txtReg]").prop("disabled", bloquear);
            $("[id*=ddlReg]").prop("disabled", bloquear);
        },
        //
        NewRegistro: function () {
            Registro.CleanRegistro();
            Registro.Codigo = 0;
            Registro.Edicion = false;
            dialogRegistro.modal("show");
        },
        //
        CheckRegistro: function () {
            if (!PROYECTOMED.CheckLength('txtRegDNI', 'Debe ingresar un valor en el campo DNI', 1))
                return false;

            if (!PROYECTOMED.CheckLength('txtRegDNI', 'El campo DNI debe tener 8 dígitos', 8, 8))
                return false;

            if (!PROYECTOMED.CheckLength('txtRegNOMBRES', 'Debe ingresar un valor en el campo NOMBRES', 1))
                return false;

            var apPat = $("[id$=txtRegAPELLIDO_PATERNO]").val();
            var apMat = $("[id$=txtRegAPELLIDO_MATERNO]").val();
            if (apPat.trim() == "" && apMat.trim() == "") {
                $("[id$=txtRegAPELLIDO_PATERNO]").addClass("ui-state-error");
                $("[id$=txtRegAPELLIDO_MATERNO]").addClass("ui-state-error");
                PROYECTOMED.MostrarMensaje('Debe ingresar un valor en el campo APELLIDO PATERNO o APELLIDO MATERNO', 'Error');
                $("[id$=txtUsuAPELLIDO_PATERNO]").focus();
                return false;
            } else {
                $("[id$=txtRegAPELLIDO_PATERNO]").removeClass("ui-state-error");
                $("[id$=txtRegAPELLIDO_MATERNO]").removeClass("ui-state-error");
            }

            if (!PROYECTOMED.CheckLength('ddlRegHORARIO_LABORAL', 'Debe seleccionar un valor en el campo HORARIO LABORAL', 1))
                return false;

            if ($("[id$=txtRegFECHA_NACIMIENTO]").val() != "") {
                if (!PROYECTOMED.CheckDate('txtRegFECHA_NACIMIENTO', 'Debe ingresar un valor correcto en el campo FECHA NACIMIENTO (DD/MM/YYYY)'))
                    return false;
            }

            if ($("[id$=txtRegCORREO]").val() != "") {
                if (!PROYECTOMED.CheckEmail('txtRegCORREO', 'Debe ingresar un valor correcto en el campo CORREO ELECTRÓNICO'))
                    return false;
            }



            if ($("[id$=txtRegFECHA_INGRESO]").val() != "") {
                if (!PROYECTOMED.CheckDate('txtRegFECHA_INGRESO', 'Debe ingresar un valor correcto en el campo FECHA INGRESO (DD/MM/YYYY)'))
                    return false;
            }
            return true;

        },
        //
        GetRegistro: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TRABAJADOR = ret.cod;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetRegistro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        Registro.CleanRegistro();
                        Registro.Codigo = datTabla.ID_TRABAJADOR;
                        Registro.Edicion = true;
                        $("[id$=txtRegDNI]").val(datTabla.NUMERO_DNI);
                        $("[id$=txtRegNOMBRES]").val(datTabla.NOMBRES);
                        $("[id$=txtRegAPELLIDO_PATERNO]").val(datTabla.APELLIDO_PATERNO);
                        $("[id$=txtRegAPELLIDO_MATERNO]").val(datTabla.APELLIDO_MATERNO);
                        $("[id$=ddlRegGENERO]").val(datTabla.ID_GENERO);
                        $("[id$=ddlRegHORARIO_LABORAL]").val(datTabla.ID_HORARIO_LABORAL);
                        $("[id$=txtRegFECHA_NACIMIENTO]").val(datTabla.FECHA_NACIMIENTO_TEXTO);
                        $("[id$=txtRegCORREO]").val(datTabla.CORREO_ELECTRONICO);
                        $("[id$=txtRegTELEFONO]").val(datTabla.TELEFONO);
                        $("[id$=txtRegFECHA_INGRESO]").val(datTabla.FECHA_INGRESO_TEXTO);
                        $("[id$=txtRegDIRECCION]").val(datTabla.DIRECCION);
                        $("[id$=ddlRegDEPARTAMENTO]").val(datTabla.DEP_CODIGO);

                        var datParam = new Object();
                        datParam.DEP_CODIGO = $("[id$=ddlRegDEPARTAMENTO]").val();
                        PROYECTOMED.CargarCombo(PROYECTOMED.UrlActionMethod("trabajador", "GetProvincias"), "ddlRegPROVINCIA", { 'dato': datParam }, "(SELECCIONAR)", function () {
                            $("[id$=ddlRegPROVINCIA]").val(datTabla.PRV_CODIGO);
                            datParam.PRV_CODIGO = $("[id$=ddlRegPROVINCIA]").val();
                            PROYECTOMED.CargarCombo(PROYECTOMED.UrlActionMethod("trabajador", "GetDistritos"), "ddlRegDISTRITO", { 'dato': datParam }, "(SELECCIONAR)", function () {
                                $("[id$=ddlRegDISTRITO]").val(datTabla.DIS_CODIGO);
                            });
                        });

                        dialogRegistro.modal("show");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },
        //
        InitCaptura: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TRABAJADOR = ret.cod;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("IniCaptura"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        Registro.IdCaptura = ret.cod;
                        Registro.DniCaptura = ret.dni;
                        if (!intervalo) {
                            intervalo = setInterval(Registro.ValidaCaptura, 2000);
                            $("#seguimiento_log").html("Espere un momento...<br>");
                            $('#btnLogAceptar').prop('disabled', true);
                        }
                        dialogLog.modal("show");
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },
        //
        ShowSeguimiento: function () {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.LOG_DNI = Registro.DniCaptura;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetLog"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Lista;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#seguimiento_log").html("Espere un momento...<br>");
                        var cadena = "Espere un momento...<br>";
                        for (var e = 0; e < datTabla.length; e++) {
                            cadena = cadena + "<strong>" + datTabla[e].LOG_FECHA_HORA + "</strong>" + ' ' + datTabla[e].LOG_COMENTARIO + "<br>";
                        }
                        $("#seguimiento_log").html(cadena);

                        //Registro.ValidaCaptura();
                    } else {
                        //PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                    
                }
            });
        },

        //
        ValidaCaptura: function () {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TRABAJADOR = Registro.IdCaptura;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetRegistro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        if (datTabla.INICIA_CAPTURA == "0") {
                            Registro.ShowSeguimientoFinal(datTabla.NUMERO_DNI);
                        } else {
                            Registro.ShowSeguimiento(datTabla.NUMERO_DNI);
                        }
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },

        //
        ShowSeguimientoFinal: function (dni) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.LOG_DNI = dni;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("GetLog"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Lista;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#seguimiento_log").html("Espere un momento...<br>");
                        var cadena = "Espere un momento...<br>";
                        for (var e = 0; e < datTabla.length; e++) {
                            cadena = cadena + "<strong>" + datTabla[e].LOG_FECHA_HORA + "</strong>" + ' ' + datTabla[e].LOG_COMENTARIO + "<br>";
                        }
                        $("#seguimiento_log").html(cadena);

                        clearInterval(intervalo);
                        intervalo = null;
                        $('#btnLogAceptar').prop('disabled', false);
                    } else {
                        //PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }

                }
            });
        },


        ////
        //ShowSeguimiento: function () {
        //    var datParam = new Object();
        //    datParam.TOKEN = PROYECTOMED.GetToken();
        //    datParam.ID_TRABAJADOR = Registro.IdCaptura;

        //    $.ajax({
        //        type: "POST",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        url: PROYECTOMED.UrlActionMethod("GetRegistro"),
        //        data: PROYECTOMED.SetParam({ 'dato': datParam }),
        //        success: function (result) {
        //            result = PROYECTOMED.GetResult(result);
        //            var datTabla = result.Dato;
        //            var msgTabla = result.Msg;
        //            if (datTabla) {
        //                //aqui muestra el contenido de la lista
        //                Registro.ListaSeguimiento(datTabla.NUMERO_DNI);

        //                if (datTabla.INICIA_CAPTURA == "0") {
        //                    Registro.ListaSeguimiento(datTabla.NUMERO_DNI);
        //                    clearInterval(intervalo);
        //                    intervalo = null;
        //                    $('#btnLogAceptar').prop('disabled', false);
        //                }

        //            } else {
        //                PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
        //            }
        //        }
        //    });
        //},
        ////
        //ListaSeguimiento: function (dni) {
        //    var datParam = new Object();
        //    datParam.TOKEN = PROYECTOMED.GetToken();
        //    datParam.LOG_DNI = dni;

        //    $.ajax({
        //        type: "POST",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        url: PROYECTOMED.UrlActionMethod("GetLog"),
        //        data: PROYECTOMED.SetParam({ 'dato': datParam }),
        //        success: function (result) {
        //            result = PROYECTOMED.GetResult(result);
        //            var datTabla = result.Lista;
        //            var msgTabla = result.Msg;
        //            if (datTabla) {
        //                $("#seguimiento_log").html("Espere un momento...<br>");
        //                var cadena = "Espere un momento...<br>";
        //                for (var e = 0; e < datTabla.length; e++) {
        //                    cadena = cadena + datTabla[e].LOG_FECHA_HORA + ' ' + datTabla[e].LOG_COMENTARIO + "<br>";
        //                }
        //                $("#seguimiento_log").html(cadena);
        //            } else {
        //               //PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
        //            }

        //        }
        //    });
        //},
        //
        UpdRegistro: function () {
            if (!Registro.CheckRegistro())
                return false;

            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TRABAJADOR = Registro.Codigo;
            datParam.NUMERO_DNI = $("[id$=txtRegDNI]").val();
            datParam.NOMBRES = $("[id$=txtRegNOMBRES]").val();
            datParam.APELLIDO_PATERNO = $("[id$=txtRegAPELLIDO_PATERNO]").val();
            datParam.APELLIDO_MATERNO = $("[id$=txtRegAPELLIDO_MATERNO]").val();
            datParam.ID_GENERO = $("[id$=ddlRegGENERO]").val();
            datParam.ID_HORARIO_LABORAL = $("[id$=ddlRegHORARIO_LABORAL]").val();
            datParam.FECHA_NACIMIENTO_TEXTO = $("[id$=txtRegFECHA_NACIMIENTO]").val();
            datParam.CORREO_ELECTRONICO = $("[id$=txtRegCORREO]").val();
            datParam.TELEFONO = $("[id$=txtRegTELEFONO]").val();
            datParam.FECHA_INGRESO_TEXTO = $("[id$=txtRegFECHA_INGRESO]").val();
            datParam.DEP_CODIGO = $("[id$=ddlRegDEPARTAMENTO]").val();
            datParam.PRV_CODIGO = $("[id$=ddlRegPROVINCIA]").val();
            datParam.DIS_CODIGO = $("[id$=ddlRegDISTRITO]").val();
            datParam.DIRECCION = $("[id$=txtRegDIRECCION]").val();

            PROYECTOMED.Ajax({
                url: PROYECTOMED.UrlActionMethod((Registro.Edicion ? "Upd" : "Ins") + "Registro"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                tokenRequired: true,
                success: function (result) {
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        $("#tblData").trigger("reloadGrid");
                        dialogRegistro.modal("hide");
                        if (msgTabla)
                            PROYECTOMED.MostrarMensaje(msgTabla, 'Suceso');
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });

        },
        //
        DelRegistro: function (ret) {
            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.ID_TRABAJADOR = ret.cod;

            PROYECTOMED.MostrarPopup('Borrar', '¿Seguro de borrar el registro?', 'SiNo', function () {
                PROYECTOMED.Ajax({
                    url: PROYECTOMED.UrlActionMethod("DelRegistro"),
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
    }
    //-----------grillas-------------
    $("#tblData").jqGrid({
        datatype: function () {
            var parFiltro = PROYECTOMED.GetPaginationJQGrid('tblData');
            parFiltro.PARAMETRO1 = $("#txtFilDNI").val();
            parFiltro.PARAMETRO2 = $("#txtFilAPELLIDO_PATERNO").val();
            parFiltro.PARAMETRO3 = $("#txtFilAPELLIDO_MATERNO").val();

            PROYECTOMED.CargarGridLoad('GetBandeja', 'tblData', { 'dato': parFiltro });

        },
        colNames: ['ID_TRABAJADOR', 'N° DNI', 'NOMBRES', 'APELLIDO PATERNO', 'APELLIDO MATERNO', 'N° CELULAR', 'CORREO ELECTRÓNICO', 'FECHA INGRESO', 'ACCIONES'],
        colModel: [
            { name: 'cod', index: 'cod', align: 'left', width: 5, sortable: false, hidden: true },
            { name: 'dni', index: 'dni', align: 'center', width: 10, sortable: false, hidden: false },
            { name: 'c04', index: 'c04', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c05', index: 'c05', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c06', index: 'c06', align: 'left', width: 14, sortable: false, hidden: false },
            { name: 'c07', index: 'c07', align: 'left', width: 10, sortable: false, hidden: false },
            { name: 'c08', index: 'c08', align: 'left', width: 16, sortable: false, hidden: false },
            { name: 'c07', index: 'c07', align: 'center', width: 10, sortable: false, hidden: false },
            { name: 'c09', index: 'c09', align: 'center', width: 12, sortable: false, hidden: false, formatter: VerAccionesRegistro },
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

    function VerAccionesRegistro(cellvalue, options, rowObject) {
        var html = "";
        html = "<div class='tooltip-demo'>";
        html = html + "<button class='btnRegCap btn btn-in-grid' type='button'  id='btnRegCap" + options.rowId + "' data-original-title='Capturar' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-photo' ></i></button>";
        html = html + "<button class='btnRegEdit btn btn-in-grid' type='button'  id='btnRegEdit" + options.rowId + "' data-original-title='Editar' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-pencil' ></i></button>";
        html = html + "<button class='btnRegDel btn btn-in-grid' type='button'  id='btnRegDel" + options.rowId + "' data-original-title='Eliminar' data-toggle='tooltip' data-placement='bottom'><i class='fa fa-trash-o' ></i></button>";
        html = html + "</div>";
        return html;
    }

    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-x', 'hidden');
    $('#contData .ui-jqgrid .ui-jqgrid-bdiv').css('overflow-y', 'hidden');

    iniProcess = true;

});

/// <reference path="../utils/PROYECTOMED.js" />

//window.setInterval(
//    function () {
//        GetFecha();
//    }
////     Intervalo de tiempo 1000 = 1 SEGUNDO
//    , 100000);

function GetFecha() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: PROYECTOMED.UrlActionMethod("Home", "GetFecha"),
        data: PROYECTOMED.SetParam({}),
        success: function (result) {
            result = PROYECTOMED.GetResult(result);
            var datTabla = result.Dato;
            var msgTabla = result.Msg;
            if (datTabla) {
            } else {
                //PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
            }
        },
    });
}

$(document).ready(function () {
    /*$.sessionTimeout({
        title: 'Aterta de tiempo de sesión!',
        message: 'Su sesión está a punto de expirar.',
        keepAliveUrl: 'PaginaInicial.aspx',
        keepAlive: false,
        ignoreUserActivity: true,
        logoutUrl: "../Login.aspx",
        warnAfter: 10000, // tiempo faltante desde q aparece la alerta
        redirAfter: 20000, // tiempo en el que redirecciona
        countdownMessage: 'Usted perderá su sesión en {timer} segundos.',
        logoutButton: "Cerrar Sesión",
        keepAliveButton: "Permanecer conectado",
        onRedir: function () {
            Registro.Salir();
        },
    });*/

    $(":input").attr("autocomplete", "off");

    var dialogContrasenia = $("#dialogContrasenia");
    var esRolSoloProd = $("[id$=hfSoloProducto]").val();
    var esRolFinanza = $("[id$=hfFinancia]").val();
    var dialogCerrar = $("#dialogCerrar");
    dialogContrasenia.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) { });
    dialogCerrar.modal({ keyboard: false, backdrop: 'static', show: false }).on('shown.bs.modal', function (e) { });

    $("[id$=txtSGCSisACTUAL]").SoloPassword(); PROYECTOMED.ToPasswordSinEspaciosDobles("txtSGCSisACTUAL");
    $("[id$=txtSGCSisNUEVA]").SoloPassword(); PROYECTOMED.ToPasswordSinEspaciosDobles("txtSGCSisNUEVA");
    $("[id$=txtSGCSisCONFIRMAR]").SoloPassword(); PROYECTOMED.ToPasswordSinEspaciosDobles("txtSGCSisCONFIRMAR");

    var modalAbierto = false;
    //tiempo de inactividad despues de 5 minutos
    var mouseStop = null;
    var Time = 1200000; //600000 = 10 MINUTOS
    $(document).on('mousemove', function () {
        if (modalAbierto == false) {
            clearTimeout(mouseStop);
            mouseStop = setTimeout(Myfunction, Time);
        }
    });

    var tiempo_limite;
    function Myfunction() {
        //aqui efectua la funcion cuando dejas de mover el raton despeus del Time indicado
        dialogCerrar.modal("show");
        modalAbierto = true;
        tiempo_limite = new Date();
        tiempo_limite.setSeconds(tiempo_limite.getSeconds() + 120.5); //120.5
        $('#reloj').countdown('option', { until: tiempo_limite });
    }

    $('#reloj').countdown({
        until: tiempo_limite,
        onExpiry: FinConteo, onTick: watchCountdown, format: 'MS'
    });

    function FinConteo() {
        if (modalAbierto) {
            modalAbierto == false;
            document.getElementById('logoutForm').submit();
        }       
    }

    function watchCountdown(periods) {
        $('#conteo').text(periods[5] + ' minuto(s) y ' +
            periods[6] + ' segundo(s)');
    }

    $("[id$=btnSesCancelar]").click(function () {
        modalAbierto = false;
        document.getElementById('logoutForm').submit();
        return false;
    });

    $("[id$=btnSesAceptar]").click(function () {
        modalAbierto = false;
        dialogCerrar.modal("hide");
        return false;
    });



    $("#ribbon-tab-header-0100").click(function () {
        //Registro.SetTab("1");
        PROYECTOMED.ConfigTab("0100");
    });

    $("#ribbon-tab-header-0200").click(function () {
        //Registro.SetTab("2");
        PROYECTOMED.ConfigTab("0200");
    });

    $("#ribbon-tab-header-0300").click(function () {
        //Registro.SetTab("3");
        PROYECTOMED.ConfigTab("0300");
    });

    $("#ribbon-tab-header-0400").click(function () {
        //Registro.SetTab("4");
        PROYECTOMED.ConfigTab("0400");
    });

    $("#ribbon-tab-header-0500").click(function () {
        //Registro.SetTab("5");
        PROYECTOMED.ConfigTab("0500");
    });

    $("#ribbon-tab-header-0600").click(function () {
        //Registro.SetTab("5");
        PROYECTOMED.ConfigTab("0600");
    });

    $("#ribbon-tab-header-0700").click(function () {
        //Registro.SetTab("5");
        PROYECTOMED.ConfigTab("0700");
    });

    $("[id$=btnSalir]").click(function () {
        PROYECTOMED.MostrarPopup("Cerrar Sesión", "¿Está seguro de salir del sistema?", "SiNo", function () {
            document.getElementById('logoutForm').submit();
        });
        return false;
    });

    $("[id$=btnCambiar]").click(function () {
        Registro.LimpiarCambio();
        dialogContrasenia.modal("show");
        return false;
    });

    $("[id*=btnSGCSis]").click(function () {
        var id = $(this).attr("id");
        var idText = id.replace("btn", "txt");
        var x = document.getElementById(idText);
        if (x.type === "password") {
            x.type = "text";
            $("#" + id + " i").removeClass("fa-eye");
            $("#" + id + " i").addClass("fa-eye-slash");
        } else {
            x.type = "password";
            $("#" + id + " i").addClass("fa-eye");
            $("#" + id + " i").removeClass("fa-eye-slash");
        }
        return false;
    });

    $("[id$=btnCSGCSisAceptar]").click(function () {
        Registro.CambiarContraseña();
        return false;
    });

    var Registro = {
        //
        Salir: function () {
            var token = "";
            $.ajax({
                url: "MetodosGenericos.aspx/ExitApp",
                data: JSON.stringify({ 'token': token }),
                success: function (result) {
                    var datForm = result.d.Dato;
                    var msgForm = result.d.Msg;
                    if (datForm) {
                        //window.open(datForm, '_blank');
                        var backlen = history.length;
                        history.go(-backlen);
                        window.location.href = "../Login.aspx";
                    } else {
                        PROYECTOMED.MostrarMensaje(msgForm, 'Error');
                    }
                }
            });
            return false;
        },
        //
        SetTab: function (id) {
            var parData = new Object();
            var token = '';

            parData.parametro1 = id;

            $.ajax({
                url: "MetodosGenericos.aspx/SetTab",
                data: JSON.stringify({ 'token': token, 'dato': parData }),
                success: function (result) {
                    var datData = result.d.Dato;
                    var msgData = result.d.Msg;
                    if (datData) {
                        location.href = datData;
                    } else {
                        if (msgData)
                            PROYECTOMED.MostrarMensaje(msgData);
                    }
                }
            });
        },
        //
        SetRecurso: function (id) {
            var parData = new Object();
            var token = '';

            parData.parametro1 = id;

            $.ajax({
                url: "MetodosGenericos.aspx/SetRecurso",
                data: JSON.stringify({ 'token': token, 'dato': parData }),
                success: function (result) {
                    var datData = result.d.Dato;
                    var msgData = result.d.Msg;
                    if (datData) {
                    } else {
                        if (msgData)
                            PROYECTOMED.MostrarMensaje(msgData);
                    }
                }
            });
        },
        //
        ConfigurarTabla: function (id, codigo, href) {

            var parData = new Object();
            var token = '';

            parData.parametro1 = id;
            parData.parametro2 = codigo;
            parData.parametro3 = href;
            $.ajax({
                url: "MetodosGenericos.aspx/SetTabla",
                data: JSON.stringify({ 'token': token, 'dato': parData }),
                success: function (result) {
                    var datData = result.d.Dato;
                    var msgData = result.d.Msg;
                    if (datData) {
                        location.href = datData;
                    } else {
                        if (msgData)
                            PROYECTOMED.MostrarMensaje(msgData);
                    }
                }
            });
        },
        //
        CambiarContraseña: function () {
            if (!Registro.ValidarCambio())
                return;

            var datParam = new Object();
            datParam.TOKEN = PROYECTOMED.GetToken();
            datParam.USUARIO = $("[id$=txtSGCUsuUSUARIO]").val();
            datParam.CONTRASENIA = $("[id$=txtSGCSisACTUAL]").val();
            datParam.NUEVA_CONTRASENIA = $("[id$=txtSGCSisNUEVA]").val();
            datParam.CONFIRMAR_NUEVA_CONTRASENIA = $("[id$=txtSGCSisCONFIRMAR]").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: PROYECTOMED.UrlActionMethod("Account", "UpdPassword"),
                data: PROYECTOMED.SetParam({ 'dato': datParam }),
                success: function (result) {
                    result = PROYECTOMED.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        dialogContrasenia.modal("hide");
                        PROYECTOMED.MostrarPopup("Sistema de Gestión de Contratos", "Se actualizó la contraseña con éxito.<br>Se cerrará la sesión iniciada para que vuelva a acceder con su nueva contraseña.", "Aceptar", function () {
                            var backlen = history.length;
                            history.go(-backlen);
                            //window.location.href = "/Account/Login";
                            document.getElementById('logoutForm').submit();
                        });
                    } else {
                        PROYECTOMED.MostrarMensaje(msgTabla, 'Error');
                    }
                }
            });
        },
        //
        ValidarCambio: function () {
            if (!PROYECTOMED.CheckLength('txtSGCSisACTUAL', 'Debe ingresar un valor en el campo CONTRASEÑA ACTUAL', 1))
                return false;
            if (!PROYECTOMED.CheckLength('txtSGCSisNUEVA', 'Debe ingresar un valor en el campo NUEVA CONTRASEÑA', 1))
                return false;
            if (!PROYECTOMED.CheckLength('txtSGCSisCONFIRMAR', 'Debe ingresar un valor en el campo CONFIRMAR NUEVA CONTRASEÑA', 1))
                return false;
            /*if ($("[id$=txtSGCSisACTUAL]").val() == $("[id$=txtSGCSisNUEVA]").val()) {
                PROYECTOMED.MostrarMensaje("La NUEVA CONTRASEÑA debe ser diferente a la CONTRASEÑA ACTUAL", "Error");
                return false;
            }
            if ($("[id$=txtSGCSisNUEVA]").val() != $("[id$=txtSGCSisCONFIRMAR]").val()) {
                PROYECTOMED.MostrarMensaje("La NUEVA CONTRASEÑA no coincide con la CONFIRMACIÓN", "Error");
                return false;
            }*/
            return true;
        },
        //
        LimpiarCambio: function () {
            $("[id*='txtSGCSis']").removeClass('ui-state-error');
            $("[id*='txtSGCSis']").val('');
        },

    }

});

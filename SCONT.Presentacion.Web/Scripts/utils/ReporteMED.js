$(document).ready(function () {
    $("[id$=btnReporte]").click(function () {
        Registro.GeneraReporte();
        return false;
    });


    var Registro = {

        //
        GeneraReporte: function () {
            ReportPanelShow({
                url: "/Reporte/Reporte",
                data: {
                    "FILE_FORMAT": "pdf"
                },
                success: function () {
                    $("button").prop("disabled", false);
                }
            });

        },
    }



    /************************REPORT PANEL***********************************/
    function ReportPanelShow(option) {
        //********************************************************
        var form = $('<form>', { action: "http://localhost:41510/reporte" + option.url, method: 'post' });
        if (option.data != undefined || option.data != null) {
            var args = option.data;
            //if (CONF.PARAMETROS.ENCRIPTACION) {
            //    args = {
            //        filter: Encrypt(JSON.stringify(option.data), CONF.SEGURIDAD.KEY_CODE())
            //    };
            //}
            $.each(args, function (key, value) {
                $(form).append($('<input>', { type: 'hidden', name: key, value: value }));
            });
        }
        $(form).appendTo('body');

        if (window.File && window.FileReader && window.FileList && window.Blob) {


            //********************************************
            var xhr = new XMLHttpRequest();
            xhr.responseType = 'blob';
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 2) {
                    if (xhr.getResponseHeader("Content-Type") != null) {//No deberia retornar null, deberia retornar un tipo de error en JSON
                        if (xhr.getResponseHeader("Content-Type").indexOf("application/json") >= 0) {
                            xhr.responseType = "json";
                        }
                    }

                }
            };
            xhr.onload = function (event) {

                if (xhr.status === 200) {
                    var filename = "";
                    var disposition = xhr.getResponseHeader('Content-Disposition');
                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                        var matches = filenameRegex.exec(disposition);
                        if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                    }

                    if (xhr.responseType == "json") {

                        //if (CONF.PARAMETROS.ENCRIPTACION) {
                        //    var decrypted = Decrypt(xhr.response, CONF.SEGURIDAD.KEY_CODE());
                        //    if (isJSON(decrypted)) {
                        //        var responseJson = JSON.parse(decrypted);

                        //        if (responseJson.statusCode === CONF.CONSTANTE.HTTP_STATUS_CODE.SESSION_NO_FOUND) {
                        //            MessageAlert({
                        //                icon: "warning",
                        //                title: "Advertencia de Sesión",
                        //                message: "Se ha terminado la sesión de usuario. Por favor vuelva a ingresar",
                        //                fnOk: function () {
                        //                    window.location.href = CONF.PARAMETROS.APPLICATION_PATH + "/App/Login";
                        //                }
                        //            });
                        //        };
                        //    }
                        //}
                    } else {
                        var urlReportResponse = window.URL.createObjectURL(xhr.response); // xhr.response is a blob
                        /////////////////////////////////////////////////////////////////////////
                        if (xhr.response.size > 0) {
                            $("#divReportPanel iframe").ready(function () {
                                $("#divReportPanel div.progressReport").hide();
                            });
                            $("#divReportPanel iframe").attr('src', urlReportResponse);
                            $("#divReportPanel div.progressReport").show();

                            $("#divReportPanel").modal({
                                backdrop: 'static',
                                keyboard: false,
                                show: true
                            });
                        } else {

                        }

                        /////////////////////////////////////////////////////////////////////////
                    }

                }
            };
            xhr.onerror = function (event) {

            };

            //*******parameters*******//
            xhr.open('POST', "http://localhost:41510/reporte" + option.url, true);
            xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            xhr.send($(form).serialize());
            //******************///
        }

        delete form;
    };

});

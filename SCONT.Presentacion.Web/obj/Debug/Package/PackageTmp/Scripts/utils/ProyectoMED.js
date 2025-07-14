/// <reference path="../vendor/jquery-1.11.1.min.js" />
/// <reference path="../jquery.jqgrid.min.js" />

//

// Retorna un entero aleatorio entre min (incluido) y max (excluido)
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

function addDays(dateString, days) {
    var result = new Date(PROYECTOMED.StringToDate(dateString));
    result.setDate(result.getDate() + parseFloat(days));

    var resultado = new Date(result);
    var dd = resultado.getDate();
    var mm = resultado.getMonth() + 1; //January is 0!
    var yyyy = resultado.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var resultado_string = dd + '/' + mm + '/' + yyyy;
    return resultado_string;
}

function getDateNow() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = dd + '/' + mm + '/' + yyyy;

    return today;
}

function toDatePicker(str) {
    var dia = str.substr(0, 2);
    var mes = str.substr(3, 2) - 1;
    var anho = str.substr(6, 4);
    return new Date(anho, mes, dia);
}

function strPad(i, l, s) {
    var o = i.toString();
    if (!s) { s = '0'; }
    while (o.length < l) {
        o = s + o;
    }
    return o;
};
function parseJsonDate(jsonDate) {
    if (jsonDate == null)
        return '';
    var value = new Date(parseInt(jsonDate.substr(6)));
    var ret = strPad(value.getDate(), 2) + "/" + strPad(value.getMonth() + 1, 2) + "/" + value.getFullYear();
    return ret;
}

function FormatoSeparadorMiles(nStr, sepMil, sepDec, cantDec) {
    var decimal = parseFloat(nStr);
    if (cantDec == undefined) { cantDec = 2; }
    if (cantDec > 0) { nStr = decimal.toFixed(cantDec); }
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? sepDec + (x[1] + '000000').substring(0, cantDec) : sepDec + PROYECTOMED.Right('0000000000', cantDec);
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + sepMil + '$2');
    }
    return x1 + x2;
}

function DateToStringDDMMYYYY(date) {
    var resultado = new Date();
    resultado = date;
    var dd = resultado.getDate();
    var mm = resultado.getMonth() + 1; //January is 0!
    var yyyy = resultado.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var resultado_string = dd + '/' + mm + '/' + yyyy;
    return resultado_string;
}

function Unidades(num) {

    switch (num) {
        case 1: return 'UN';
        case 2: return 'DOS';
        case 3: return 'TRES';
        case 4: return 'CUATRO';
        case 5: return 'CINCO';
        case 6: return 'SEIS';
        case 7: return 'SIETE';
        case 8: return 'OCHO';
        case 9: return 'NUEVE';
    }

    return '';
}//Unidades()

function Decenas(num) {

    let decena = Math.floor(num / 10);
    let unidad = num - (decena * 10);

    switch (decena) {
        case 1:
            switch (unidad) {
                case 0: return 'DIEZ';
                case 1: return 'ONCE';
                case 2: return 'DOCE';
                case 3: return 'TRECE';
                case 4: return 'CATORCE';
                case 5: return 'QUINCE';
                default: return 'DIECI' + Unidades(unidad);
            }
        case 2:
            switch (unidad) {
                case 0: return 'VEINTE';
                default: return 'VEINTI' + Unidades(unidad);
            }
        case 3: return DecenasY('TREINTA', unidad);
        case 4: return DecenasY('CUARENTA', unidad);
        case 5: return DecenasY('CINCUENTA', unidad);
        case 6: return DecenasY('SESENTA', unidad);
        case 7: return DecenasY('SETENTA', unidad);
        case 8: return DecenasY('OCHENTA', unidad);
        case 9: return DecenasY('NOVENTA', unidad);
        case 0: return Unidades(unidad);
    }
}//Unidades()

function DecenasY(strSin, numUnidades) {
    if (numUnidades > 0)
        return strSin + ' Y ' + Unidades(numUnidades)

    return strSin;
}//DecenasY()

function Centenas(num) {
    let centenas = Math.floor(num / 100);
    let decenas = num - (centenas * 100);

    switch (centenas) {
        case 1:
            if (decenas > 0)
                return 'CIENTO ' + Decenas(decenas);
            return 'CIEN';
        case 2: return 'DOSCIENTOS ' + Decenas(decenas);
        case 3: return 'TRESCIENTOS ' + Decenas(decenas);
        case 4: return 'CUATROCIENTOS ' + Decenas(decenas);
        case 5: return 'QUINIENTOS ' + Decenas(decenas);
        case 6: return 'SEISCIENTOS ' + Decenas(decenas);
        case 7: return 'SETECIENTOS ' + Decenas(decenas);
        case 8: return 'OCHOCIENTOS ' + Decenas(decenas);
        case 9: return 'NOVECIENTOS ' + Decenas(decenas);
    }

    return Decenas(decenas);
}//Centenas()

function Seccion(num, divisor, strSingular, strPlural) {
    let cientos = Math.floor(num / divisor)
    let resto = num - (cientos * divisor)

    let letras = '';

    if (cientos > 0)
        if (cientos > 1)
            letras = Centenas(cientos) + ' ' + strPlural;
        else
            letras = strSingular;

    if (resto > 0)
        letras += '';

    return letras;
}//Seccion()

function Miles(num) {
    let divisor = 1000;
    let cientos = Math.floor(num / divisor)
    let resto = num - (cientos * divisor)

    let strMiles = Seccion(num, divisor, 'UN MIL', 'MIL');
    let strCentenas = Centenas(resto);

    if (strMiles == '')
        return strCentenas;

    return strMiles + ' ' + strCentenas;
}//Miles()

function Millones(num) {
    let divisor = 1000000;
    let cientos = Math.floor(num / divisor)
    let resto = num - (cientos * divisor)

    let strMillones = Seccion(num, divisor, 'UN MILLON', 'MILLONES');
    let strMiles = Miles(resto);

    if (strMillones == '')
        return strMiles;

    return strMillones + ' ' + strMiles;
}//Millones()

function NumeroALetras(num) {

    let data = {
        numero: num,
        enteros: Math.floor(num),
        centavos: (((Math.round(num * 100)) - (Math.floor(num) * 100))),
        letrasCentavos: '',
        letrasMonedaPlural: 'SOLES',
        letrasMonedaSingular: 'SOL',
        letrasMonedaCentavoPlural: '/100',
        letrasMonedaCentavoSingular: '/100'
    };

    if (data.centavos >= 0) {
        data.letrasCentavos = 'Y ' + (function () {
            if (data.centavos <= 1)
                return data.centavos +  data.letrasMonedaCentavoSingular;
            else
                return data.centavos +  data.letrasMonedaCentavoPlural;
        })();
    };

    if (data.enteros == 0)
        return 'CERO ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
    if (data.enteros == 1)
        return Millones(data.enteros) + ' ' + data.letrasCentavos + ' ' + data.letrasMonedaSingular ;
    else
        return Millones(data.enteros) + ' ' + data.letrasCentavos + ' ' + data.letrasMonedaPlural ;
};




//Inicializar
//$(function () {
//    $("form,input").bind("keypress", function (e) {
//        if (e.keyCode == 13) { return false; }
//    });

//    $.ajaxSetup({
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: {},
//        error: function (jqXHR, textStatus, errorThrown) {
//            /*
//            if (jqXHR.status == 404) {
//                alert("Element not found.");
//            } else {
//                alert("Error: " + textStatus + ": " + errorThrown);
//            }
//            */
//            document.location.href = "../Account/Presentacion";
//        }
//    });

//    $(document).ajaxError(function (e, xhr, settings, exception) {
//        document.location.href = "../Account/Presentacion";
//        /*
//        if (exception == "Unauthorized")
//            document.location.href = "../Login.aspx";
//        else
//            alert('error in: ' + settings.url + ' \\n' + 'error:\\n' + exception);
//        */
//    });
//});

//consistencia de ingreso de datos
var ctrlDown = false,
    shifDown = false,
    shifKey = 16,
    lineKey = 189,
    ctrlKey = 17,
    cmdKey = 91,
    vKey = 86,
    cKey = 67;


$(document).keydown(function (e) {
    if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = true;
    if (e.keyCode == shifKey) shifDown = true;
}).keyup(function (e) {
    if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = false;
    if (e.keyCode == shifKey) shifDown = false;
});

$.fn.TextArea = function () {
    $(this).keydown(function (e) {
        //if (e.keyCode == 13) {
        //    var pst = e.currentTarget.selectionStart;
        //    var pst2 = e.currentTarget.selectionEnd;
        //    var string_start = e.currentTarget.value.substring(0, pst);
        //    var string_end = e.currentTarget.value.substring(pst2, e.currentTarget.value.length);
        //    $(this).val(string_start + "\n" + string_end);
        //    e.currentTarget.selectionEnd = pst + 1;
        //}
    });

    $(this).blur(function (e) {
        var max = $(this).attr('maxLength');
        var length = $(this).val().length;
        if (length > max) {
            $(this).val($(this).val().substr(0, max));
        }
    });
}

$.fn.SoloNumerosDecimales = function () {
    $(this).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    break;
                case ((e.keyCode == 110 || e.keyCode == 190) && $(this).val().indexOf(".") != -1):
                    e.preventDefault();
                    break;
                case (e.shiftKey || e.ctrlKey || e.altKey):
                    e.preventDefault();
                    break;
                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9)
                        || (n == 110) || (n == 46) || (n == 190) || (n == 13)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                    )
                    ) {
                        e.preventDefault();
                    }
            }
        }
    });

    $(this).keyup(function (e) {
        var element = this;
        if (element.value.substr(0, 1) === ".") {
            var arr = element.value.split(".");
            var decimales = "";
            if (arr.length > 1)
                decimales = arr[1];
            element.value = "0." + decimales;
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val().trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje = false;
        var ultimoCaracter = "";
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (ultimoCaracter == " " && caracter == " ") {
                    continue;
                }
                if (caracter != ".") {
                    if (PROYECTOMED.SoloNumeroPermitido(caracter.charCodeAt(0))) {
                        nueva_cadena = nueva_cadena + caracter;
                    } else {
                        if (conMensaje == false) {
                            conMensaje = true;
                        }
                    }
                } else {
                    nueva_cadena = nueva_cadena + caracter;
                }
                ultimoCaracter = caracter;
            }
            $(this).val('');
            $(this).val(nueva_cadena);

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

$.fn.SoloNumerosDecimalesGrid = function () {
    $(this).keydown(function (e) {
        if (e.ctrlKey) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    break;
                case ((e.keyCode == 110 || e.keyCode == 190) && $(this).val().indexOf(".") != -1):
                    e.preventDefault();
                    break;
                case (e.shiftKey || e.altKey):
                    e.preventDefault();
                    break;
                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9)
                        || (n == 110) || (n == 46) || (n == 190) || (n == 13)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                    )
                    ) {
                        e.preventDefault();
                    }
            }
        }
    });

    $(this).keyup(function (e) {
        var element = this;
        if (element.value.substr(0, 1) === ".") {
            var arr = element.value.split(".");
            var decimales = "";
            if (arr.length > 1)
                decimales = arr[1];
            element.value = "0." + decimales;
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val().trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje = false;
        var ultimoCaracter = "";
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (ultimoCaracter == " " && caracter == " ") {
                    continue;
                }
                if (caracter != ".") {
                    if (PROYECTOMED.SoloNumeroPermitido(caracter.charCodeAt(0))) {
                        nueva_cadena = nueva_cadena + caracter;
                    } else {
                        if (conMensaje == false) {
                            conMensaje = true;
                        }
                    }
                } else {
                    nueva_cadena = nueva_cadena + caracter;
                }
                ultimoCaracter = caracter;
            }
            $(this).val('');
            $(this).val(nueva_cadena);

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}
$.fn.SoloNumerosEnteros = function () {
    $(this).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    return false;
                    break;
                    //case ((e.shiftKey) && (e.keyCode == 35)):
                    //    e.preventDefault();
                    //    break;
                    //case ((e.shiftKey) && (e.keyCode == 36)):
                    //    e.preventDefault();
                    break;
                case (e.shiftKey || e.ctrlKey || e.altKey):
                    e.preventDefault();
                    break;

                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9) || (n == 46) || (n == 13)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                    )
                    ) {
                        e.preventDefault();
                    }
            }
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val().trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje;
        conMensaje = false;
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (PROYECTOMED.SoloNumeroPermitido(caracter.charCodeAt(0))) {
                    nueva_cadena = nueva_cadena + caracter;
                } else {
                    if (conMensaje == false) {
                        conMensaje = true;
                    }
                }
            }
            $(this).val('');
            $(this).val(nueva_cadena);

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

$.fn.SoloNumerosEnterosGrid = function () {
    $(this).keydown(function (e) {
        if (e.ctrlKey) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    return false;
                    break;
                    //case ((e.shiftKey) && (e.keyCode == 35)):
                    //    e.preventDefault();
                    //    break;
                    //case ((e.shiftKey) && (e.keyCode == 36)):
                    //    e.preventDefault();
                    break;
                case (e.shiftKey || e.altKey):
                    e.preventDefault();
                    break;

                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9) || (n == 46) || (n == 13)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                    )
                    ) {
                        e.preventDefault();
                    }
            }
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val().trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje;
        conMensaje = false;
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (PROYECTOMED.SoloNumeroPermitido(caracter.charCodeAt(0))) {
                    nueva_cadena = nueva_cadena + caracter;
                } else {
                    if (conMensaje == false) {
                        conMensaje = true;
                    }
                }
            }
            $(this).val('');
            $(this).val(nueva_cadena);

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

$.fn.SoloAlfaNumerico = function (conEspacio, inGrid) {
    if (inGrid == null || inGrid == undefined) {
        inGrid = false;
    }

    $(this).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    break;
                //case ((e.shiftKey) && (e.keyCode == 35)):
                //    e.preventDefault();
                //    break;
                //case ((e.shiftKey) && (e.keyCode == 36)):
                //    e.preventDefault();
                //    break;
                //case (e.shiftKey || e.ctrlKey || e.altKey):
                //    e.preventDefault();
                //    break;
                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9) || (n == 46) || (n == 32) || (n == 0)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                        || (n >= 65 && n <= 90) // alfabeto
                        || (n == 16)
                        || (n == 17)
                        || (n == 109) // - menos
                        || (n == 110)
                        || (n == 111) // /
                        || (n == 222) // ? en firefox
                        || (n == 219) // ? en chrome
                        || (n == 220) // °
                        || (n == 172) // ° en firefox
                        || (n == 221) // ¡
                        || (n == 190) // :
                        || (n == 188) || (n == 190) || (n == 189) || (n == 173) || (n == 192)) // . - ,   ñ
                    ) {
                        e.preventDefault();
                    } else if (conEspacio == false && n == 32) {
                        e.preventDefault();
                    }

            }
        }
    });

    $(this).keyup(function (e) {
        if (inGrid) {
            $(this).val($(this).val().toUpperCase());
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val();
        texto_original = texto_original.trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje = false;
        var ultimoCaracter = "";
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (ultimoCaracter == " " && caracter == " ") {
                    continue;
                }
                if (PROYECTOMED.CaracterPermitido(caracter.charCodeAt(0))) {
                    nueva_cadena = nueva_cadena + caracter;
                    ultimoCaracter = caracter;
                } else {
                    if (conMensaje == false) {
                        conMensaje = true;
                    }
                }
            }
            $(this).val('');
            $(this).val(nueva_cadena.toUpperCase());

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

$.fn.InputDisabled = function () {
    $(this).keypress(function (event) {
        event.preventDefault();
        return false;
    });
    $(this).keydown(function (event) {
        event.preventDefault();
        return false;
    });
    $(this).keyup(function (event) {
        event.preventDefault();
        return false;
    });

    $(this).bind('contextmenu', function (e) {
        return false;
    });
}

$.fn.SoloPassword = function () {
    $(this).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    break;
                //case ((e.shiftKey) && (e.keyCode == 35)):
                //    e.preventDefault();
                //    break;
                //case ((e.shiftKey) && (e.keyCode == 36)):
                //    e.preventDefault();
                //    break;
                //case (e.shiftKey || e.ctrlKey || e.altKey):
                //    e.preventDefault();
                //    break;
                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9) || (n == 46) || (n == 32) || (n == 0)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                        || (n >= 65 && n <= 90) // alfabeto
                        || (n == 16)
                        || (n == 17)
                        || (n == 109) // - menos
                        || (n == 110)
                        || (n == 111) // /
                        || (n == 222) // ? en firefox
                        || (n == 219) // ? en chrome
                        || (n == 220) // °
                        || (n == 172) // ° en firefox
                        || (n == 221) // ¡
                        || (n == 190) // :
                        || (n == 106) // *
                        || (n == 222) || (n == 191) // {}[] chrome
                        || (n == 174) || (n == 175) // {}[] firefox
                        || (n == 188) || (n == 190) || (n == 189) || (n == 173) || (n == 192)) // . - ,   ñ
                    ) {
                        e.preventDefault();
                    } else if (n == 32) {
                        e.preventDefault();
                    }

            }
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val();
        texto_original = texto_original.trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje = false;
        var ultimoCaracter = "";
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (ultimoCaracter == " " && caracter == " ") {
                    continue;
                }
                if (PROYECTOMED.CaracterPermitidoPassword(caracter.charCodeAt(0))) {
                    nueva_cadena = nueva_cadena + caracter;
                    ultimoCaracter = caracter;
                } else {
                    if (conMensaje == false) {
                        conMensaje = true;
                    }
                }
            }
            $(this).val('');
            $(this).val(nueva_cadena);
            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

$.fn.SoloEmail = function () {
    $(this).keydown(function (e) {
        switch (true) {
            case (e.altKey):
                e.preventDefault();
                break;
            //case ((e.shiftKey) && (e.keyCode == 35)):
            //    e.preventDefault();
            //    break;
            //case ((e.shiftKey) && (e.keyCode == 36)):
            //    e.preventDefault();
            //    break;
            //case (e.shiftKey || e.ctrlKey || e.altKey):
            //    e.preventDefault();
            //    break;
            default:
                var n = e.keyCode;
                if (!((n == 8) || (n == 9) || (n == 46) || (n == 32) || (n == 0)
                    || (n >= 35 && n <= 40)
                    || (n >= 48 && n <= 57)
                    || (n >= 96 && n <= 105)
                    || (n >= 65 && n <= 90) // alfabeto
                    || (n == 16)
                    || (n == 17)
                    || (n == 109) // - menos
                    || (n == 110)
                    || (n == 189) // _
                    || (n == 222) || (n == 191) // []
                    || (n == 107) || (n == 187) // +
                    || (n == 188) || (n == 190) || (n == 189) || (n == 173) || (n == 192)) // . - ,   ñ
                ) {
                    e.preventDefault();
                } else if (n == 32) {
                    e.preventDefault();
                }

        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje;
        conMensaje = false;
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (caracter != "." && caracter != "_" && caracter != "@" && caracter != "[" && caracter != "]" && caracter != "+") {
                    if (PROYECTOMED.CaracterPermitido(caracter.charCodeAt(0))) {
                        nueva_cadena = nueva_cadena + caracter;
                    } else {
                        nueva_cadena = nueva_cadena + " ";
                        if (conMensaje == false) {
                            conMensaje = true;
                        }
                    }

                } else {
                    nueva_cadena = nueva_cadena + caracter;
                }

            }
            $(this).val('');
            $(this).val(nueva_cadena);

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}


$.fn.SoloTexto = function (conEspacio) {
    $(this).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) {
        } else {
            switch (true) {
                case ((e.shiftKey) && (e.keyCode == 35)):
                    e.preventDefault();
                    break;
                case ((e.shiftKey) && (e.keyCode == 36)):
                    e.preventDefault();
                    break;
                case (e.shiftKey || e.ctrlKey || e.altKey):
                    e.preventDefault();
                    break;
                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9) || (n == 46) || (n == 32) || (n == 0)
                        || (n >= 35 && n <= 40)
                        || (n >= 65 && n <= 90) // alfabeto
                    )
                    ) {
                        e.preventDefault();
                    } else if (conEspacio == false && n == 32) {
                        e.preventDefault();
                    }

            }
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val();
        texto_original = texto_original.trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje = false;
        var ultimoCaracter = "";
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (ultimoCaracter == " " && caracter == " ") {
                    continue;
                }
                if (PROYECTOMED.CaracterPermitido(caracter.charCodeAt(0))) {
                    nueva_cadena = nueva_cadena + caracter;
                    ultimoCaracter = caracter;
                } else {
                    if (conMensaje == false) {
                        conMensaje = true;
                    }
                }
            }
            $(this).val('');
            $(this).val(nueva_cadena.toUpperCase());

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

$.fn.SoloFecha = function () {
    $(this).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) {
        } else {
            switch (true) {
                case (e.altKey):
                    e.preventDefault();
                    return false;
                    break;
                    //case ((e.shiftKey) && (e.keyCode == 35)):
                    //    e.preventDefault();
                    //    break;
                    //case ((e.shiftKey) && (e.keyCode == 36)):
                    //    e.preventDefault();
                    break;
                case (e.ctrlKey || e.altKey):
                    e.preventDefault();
                    break;

                default:
                    var n = e.keyCode;
                    if (!((n == 8) || (n == 9) || (n == 46) || (n == 13)
                        || (n >= 35 && n <= 40)
                        || (n >= 48 && n <= 57)
                        || (n >= 96 && n <= 105)
                        || (n == 111) || (n == 55)
                    )
                    ) {
                        e.preventDefault();
                    }
            }
        }
    });

    $(this).blur(function () {
        var texto_original = $(this).val().trim();
        var longitud = texto_original.length;
        var nueva_cadena = "";
        var conMensaje;
        conMensaje = false;
        var i;
        var caracter;
        if (longitud > 0) {
            for (i = 0; i < longitud; i++) {
                caracter = texto_original.charAt(i);
                if (caracter != "/") {
                    if (PROYECTOMED.SoloNumeroPermitido(caracter.charCodeAt(0))) {
                        nueva_cadena = nueva_cadena + caracter;
                    } else {
                        if (conMensaje == false) {
                            conMensaje = true;
                        }
                    }
                } else {
                    nueva_cadena = nueva_cadena + caracter;
                }

            }
            $(this).val('');
            $(this).val(nueva_cadena);

            if (conMensaje) {
                PROYECTOMED.MostrarMensaje("Solo se insertarán los caracteres permitidos", "Error");
            }
        } else {
            $(this).val('');
        }
    });
}

//extensiones
$.jgrid.extend({
    addToolBar: function (idToolBar) {
        var idGrid = this.attr('id');
        $("#" + idToolBar).appendTo("#t_" + idGrid);
        $("#t_" + idGrid).css('height', '30px');
    }
});

$.jgrid.extend({
    addToolNota: function (idToolBar) {
        var idGrid = this.attr('id');
        $("#" + idToolBar).appendTo("#t_" + idGrid);
        $("#t_" + idGrid).css('height', '65px');
    }
});

$.jgrid.extend({
    rowColumnHeader: function (columnNameIni, textColumn, colsMerge) {

        var idGrid = this.attr('id');
        var mygrid = $("#" + idGrid);
        var colModel, i, cmi, tr = "<tr>", skip = 0, ths;

        colModel = mygrid[0].p.colModel;
        ths = mygrid[0].grid.headers;
        for (i = 0; i < colModel.length; i++) {
            cmi = colModel[i];
            if (cmi.name !== columnNameIni) {
                if (skip === 0) {
                    $(ths[i].el).attr("rowspan", "2");
                } else {
                    skip--;
                }
            } else {
                tr += '<th class="ui-state-default ui-th-ltr" colspan="' + colsMerge + '" role="columnheader">' + textColumn + '</th>';
                skip = (colsMerge - 1); // because we make colspan="3" the next 2 columns should not receive the rowspan="2" attribute
            }
        }
        tr += "</tr>";
        mygrid.closest("div.ui-jqgrid-view").find("table.ui-jqgrid-htable > thead").append(tr);

    }
});

$.jgrid.extend({
    headerWrap: function () {

        var idGrid = this.attr('id');
        var grid = $("#t_" + idGrid);

        // get the header row which contains
        headerRow = grid.closest("div.ui-jqgrid-view")
            .find("table.ui-jqgrid-htable>thead>tr.ui-jqgrid-labels");

        // increase the height of the resizing span
        resizeSpanHeight = 'height: ' + headerRow.height() + 'px !important; cursor: col-resize;';
        headerRow.find("span.ui-jqgrid-resize").each(function () {
            this.style.cssText = resizeSpanHeight;
        });

        // set position of the dive with the column header text to the middle
        rowHight = headerRow.height();
        headerRow.find("div.ui-jqgrid-sortable").each(function () {
            var ts = $(this);
            ts.css('top', (rowHight - ts.outerHeight()) / 2 + 'px');
        });

    }
});

var CONSTANTES = {
    Ubigeo_Pais_Peru: function () { return 'PER'; },
    Estado_Vigente: "1",
    Estado_Pagado: "2",
    Estado_ResolucionParcial: "3",
    Estado_ResolucionTotal: "4",
    Estado_Liquidado: "5",
    Estado_Arbitraje: "6",
    Estado_Anulado: "7",
    Estado_AnuladoError: "8",
    Estado_Eliminado: "9",

    Tipo_Penalidad_Mora: "01",
    Tipo_Penalidad_Courier: "02",
    Tipo_Penalidad_Otros: "03",
    Tipo_Penalidad_Otros_Proc: "04",
    Tipo_Penalidad_Otros_Asp: "04",
    Tipo_Penalidad_Otros_A_Uit: "1",
    Tipo_Penalidad_Otros_A_Contrato: "2",

    Tipo_Documento_Proveido_Nota_Abono: "006",
    Tipo_Documento_Proveido_Penalidad: "007",

    Tipo_Calculo_Armada_Misma_Fecha: "1",
    Tipo_Calculo_Armada_Fecha_Consecutiva: "2",

    Rol_Usuario_Adminsitrador: "1",
    Rol_Usuario_Especialista: "2",
    Rol_Usuario_Unidad_Operativa: "3",
    Rol_Usuario_Coordinador: "4",
    Rol_Usuario_Jefe_Oficina: "6",
    Rol_Usuario_Proveedor: "17",
    Rol_Usuario_Supervisor_Tercero: "18",
    Rol_Usuario_Coordinador_PyC: "19",
    Rol_Usuario_Supervisor_Certificacion: "20",
    Rol_Usuario_Especialista_Certificacion: "21",
    Rol_Usuario_Supervisor_Presupuesto: "22",
    Rol_Usuario_Especialista_Presupuesto: "23",

    Estado_Conformidad_Pendiente: "1",
    Estado_Conformidad_Conforme: "2",

    Condicion_Inicio_Asp_Fecha_Determianda: "5",
    Condicion_Inicio_Asp_Ejecucion_OCAM: "8",

    Condicion_Inicio_Asp_Suscrita_Acta: "4",
    Condicion_Inicio_Asp_Dia_Siguiente_Suscrita_Acta: "2",

    Condicion_Inicio_Ps_Fecha_Determianda: "3",

    Tipo_Incidencia_Incremento: "1",
    Tipo_Incidencia_Ampliacion_Viatico: "12",
    Tipo_Incidencia_Reduccion: "2",
    Tipo_Incidencia_Rebaja: "3",
    Tipo_Incidencia_Resolucion_Parcial: "4",
    Tipo_Incidencia_Resolucion_Total: "5",
    Tipo_Incidencia_Arbitraje: "6",
    Tipo_Incidencia_Liquidacion: "7",
    Tipo_Incidencia_Ampliacion_Plazo: "8",
    Tipo_Incidencia_Ampliacion_Plazo_PS: "6",
    Tipo_Incidencia_Incremento_PS: "1",
    Tipo_Incidencia_Incremento_Prevision_PS: "8",
    Tipo_Incidencia_Modificacion_Convencional_PS: "9",
    Tipo_Incidencia_Incremento_RMV_PS: "10",
    Tipo_Incidencia_Nulidad_PS: "3",

    Estado_Programacion_Registrado: "1",
    Estado_Programacion_Enviado_Jefe: "2",
    Estado_Programacion_Aprobado_Jefe: "3",
    Estado_Programacion_Rechazado_Jefe: "4",
    Estado_Programacion_Asignado_Especialista: "5",
    Estado_Programacion_Requerimiento_Rechazado: "6",
    Estado_Programacion_Informe_Registrado: "7",
    Estado_Programacion_Informe_Enviado: "8",
    Estado_Programacion_Informe_Aprobado: "9",
    Estado_Programacion_Informe_Rechazado: "10",
    Estado_Programacion_Requerimiento_Vinculado: "11",
    Estado_Programacion_Rechazado_UPP: "12",
    Estado_Programacion_Derivado_Procesos: "13",
    Estado_Programacion_Asignado_Supervisor: "14",
    Estado_Programacion_Observado_Supervisor: "15",
    Estado_Programacion_Observado_Especialista: "16",
    Estado_Programacion_Informe_Observado_Supervisor: "17",
    Estado_Programacion_Informe_Aprobado_Supervisor: "18",

    Tipo_Contratacion_Tercero: "10",

    Tipo_Continuidad_Periodico: "4",
    Tipo_Continuidad_Especifico: "5",

    Sistema_Contratacion_Precio_Unitario: "1",
    Sistema_Contratacion_Suma_Alzada: "2",

    Tipo_Garantia_Carta_Fianza: "1",
    Tipo_Garantia_Retencion: "2",
    Tipo_Garantia_No_Aplica: "3",

    Estado_Digital_Pendiente: "1",
    Estado_Digital_Registrado: "2",
    Estado_Digital_Generado: "3",
    Estado_Digital_Firmado: "4",
    Estado_Digital_Publicado: "5",

    Tipo_Sericio_Otros: "4",

    Estado_Tdr_Generado: "1",
    Estado_Tdr_Enviado_Proveedor: "2",
    Estado_Tdr_Propuesta_Presentada: "3",
    Estado_Tdr_Enviado_OL: "4",
    Estado_Tdr_Observado_Ofi: "5",
    Estado_Tdr_Asignado_Especialista: "6",
    Estado_Tdr_Enviado_Aprobacion_Supervisor_Tercero: "7",
    Estado_Tdr_Observado_Especialista_Tercero: "8",
    Estado_Tdr_Enviado_Aprobacion_PyC: "9",
    Estado_Tdr_Observado_OL: "10",
    Estado_Tdr_Enviado_Especialista_Carga_8: "11",
    Estado_Tdr_Observado_Coordinador_PyC: "12",
    Estado_Tdr_Enviado_Equipo_Certificacion: "13",
    Estado_Tdr_Asignado_Especialista_Certificacion: "14",
    Estado_Tdr_Enviado_Supervisor_Certificacion: "15",
    Estado_Tdr_Observado_Especialista_Certificacion: "16",
    Estado_Tdr_Enviado_Coordinacion_Presupuesto: "17",
    Estado_Tdr_Observado_Supervisor_Certificacion: "18",
    Estado_Tdr_Asignado_Especialista_Presupuesto: "19",
    Estado_Tdr_Observado_Supervisor_Presupuesto: "20",
    Estado_Tdr_Enviado_Aprobacion_Supervisor_Presupuesto: "21",
    Estado_Tdr_Observado_Especialista_Presupuesto: "22",
    Estado_Tdr_Aprobacion_Supervisor_Presupuesto: "23",
}


var PROYECTOMED = {

    //
    DevuelveDecimal: function (str, defaultValue) {
        var retValue = defaultValue;
        if (str !== null) {
            if (str.length > 0) {
                if (!isNaN(str)) {
                    retValue = parseFloat(str);
                }
            }
        }
        return retValue;
    },
    //
    ShowBlockUI: function () {
        /// <summary>
        /// Bloquea la pantalla mientras se ejecuta AJAX
        /// </summary>
        return $.blockUI({ message: '<h3>Espere un momento...</h3>' });
    },
    //
    HideBlockUI: function () {
        /// <summary>
        /// Fin de bloqueo de pantalla mientras se ejecuta AJAX
        /// </summary>
        return $.unblockUI();
    },
    //
    UrlPageMethod: function (WebMethod) {
        /// <summary>
        /// Devuelve el Url de un WebMethod (con relación a la página donde se ejecuta el script)
        /// </summary>
        /// <param name="WebMethod" type="String">Nombre del WebMethod</param>
        return window.location.pathname + '/' + WebMethod;
    },
    //
    UrlTemplate: function (FileTemplate) {
        /// <summary>
        /// Devuelve el Url de un WebMethod (con relación a la página donde se ejecuta el script)
        /// </summary>
        /// <param name="WebMethod" type="String">Nombre del WebMethod</param>
        return '../Templates/' + FileTemplate;
    },
    //
    MostrarAlerta: function (IdControl, Mensaje, Opcion) {
        /// <summary>
        /// Muestra un mensaje en un Div
        /// </summary>
        /// <param name="IdControl" type="String">Id del control donde se muestra la alerta</param>
        /// <param name="Mensaje" type="String">Mensaje a mostrar</param>
        /// <param name="Opcion" type="String">(Opcional) indica el tipo de ventana a mostrar (Default = Normal)</param>

        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        $("[id$=" + IdControl + "]").removeClass('alert-danger');
        $("[id$=" + IdControl + "]").removeClass('alert-success');
        $("[id$=" + IdControl + "]").removeClass('alert-info');
        $("[id$=" + IdControl + "]").removeClass('alert-warning');

        switch (Opcion) {
            case 'Error':
                $("[id$=" + IdControl + "]").addClass('alert-danger');
                break;
            case 'Suceso':
                $("[id$=" + IdControl + "]").addClass('alert-success');
                break;
            case 'Info':
                $("[id$=" + IdControl + "]").addClass('alert-info');
                break;
            default:
                $("[id$=" + IdControl + "]").addClass('alert-warning');
        }

        $("[id$=" + IdControl + "] > msg").text('');
        $("[id$=" + IdControl + "] > msg").append(Mensaje);
        $("[id$=" + IdControl + "]").show();

    },
    //
    OcultarAlerta: function (IdControl) {
        /// <summary>
        /// Oculta un mensaje mostrado en un Div
        /// </summary>
        /// <param name="IdControl" type="String">Id del control donde se muestra la alerta</param>
        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        $("[id$=" + IdControl + "]").hide();
    },
    //
    SoloNumeroPermitido: function (caracter) {
        if ((caracter >= 48 & caracter <= 57)) {
            return true;
        } else {
            return false;
        }
    },
    //
    CaracterPermitido: function (caracter) {
        if ((caracter >= 97 & caracter <= 122) ||
            (caracter >= 65 & caracter <= 90) ||
            (caracter >= 48 & caracter <= 57) ||
            (caracter == 32) ||
            (caracter == 10) || //enter
            (caracter == 46) || //.
            (caracter == 39) || // '
            (caracter == 17) || // [
            (caracter == 191 || caracter == 63) || // ¿?
            (caracter == 161 || caracter == 33) || // ¡!
            (caracter == 40 || caracter == 41) || // ()
            (caracter == 47) || // /
            (caracter == 37) || // %
            (caracter == 176) || // °
            (caracter == 220) || // °
            (caracter == 95) || // _
            (caracter == 58) || // :
            (caracter == 209 || caracter == 241) || //ñÑ
            (caracter == 34 || caracter == 44 || caracter == 45) || //"-,
            (caracter == 225 || caracter == 233 || caracter == 237 || caracter == 243 || caracter == 250) ||
            (caracter == 193 || caracter == 201 || caracter == 205 || caracter == 211 || caracter == 218) ||//áéíóú
            (caracter == 228 || caracter == 235 || caracter == 239 || caracter == 246 || caracter == 252) ||
            (caracter == 196 || caracter == 203 || caracter == 207 || caracter == 214 || caracter == 220)) { //äëïöü
            return true;
        } else {
            return false;
        }
    },
    //
    CaracterPermitidoPassword: function (caracter) {
        if ((caracter >= 97 & caracter <= 122) ||
            (caracter >= 65 & caracter <= 90) ||
            (caracter >= 48 & caracter <= 57) ||
            (caracter == 32) ||
            (caracter == 10) || //enter
            (caracter == 46) || //.
            (caracter == 39) || // '
            (caracter == 17) || // [
            (caracter == 191 || caracter == 63) || // ¿?
            (caracter == 161 || caracter == 33) || // ¡!
            (caracter == 40 || caracter == 41) || // ()
            (caracter == 47) || // /
            (caracter == 37) || // %
            (caracter == 176) || // °
            (caracter == 220) || // °
            (caracter == 95) || // _
            (caracter == 58) || // :
            (caracter == 209 || caracter == 241) || //ñÑ
            (caracter == 34 || caracter == 44 || caracter == 45) || //"-,
            (caracter == 35) || (caracter == 36) || (caracter == 38) || // # $ &
            (caracter == 42 || caracter == 59) || // * ;
            (caracter == 123 || caracter == 125) || // {}
            (caracter == 91 || caracter == 93) || // []

            (caracter == 225 || caracter == 233 || caracter == 237 || caracter == 243 || caracter == 250) ||
            (caracter == 193 || caracter == 201 || caracter == 205 || caracter == 211 || caracter == 218) ||//áéíóú
            (caracter == 228 || caracter == 235 || caracter == 239 || caracter == 246 || caracter == 252) ||
            (caracter == 196 || caracter == 203 || caracter == 207 || caracter == 214 || caracter == 220)) { //äëïöü
            return true;
        } else {
            return false;
        }
    },
    //
    CaracterPermitidoEmail: function (caracter) {
        if ((caracter >= 97 & caracter <= 122) ||
            (caracter >= 65 & caracter <= 90) ||
            (caracter >= 48 & caracter <= 57)) {
            return true;
        } else {
            return false;
        }
    },
    //
    MostrarMensaje: function (Mensaje, Opcion, Time) {

        /// <summary>
        /// Muestra un mensaje en la parte superior izquierda de la pantalla
        /// </summary>
        /// <param name="Mensaje" type="String">Mensaje a mostrar</param>
        /// <param name="Opcion" type="String">(Opcional) indica el tipo de ventana a mostrar (Default = Normal)</param>

        //
        //if (!Opcion) { Opcion = 'Normal'; }

        //switch (Opcion) {
        //    case 'Error':
        //        var a = noty({ text: Mensaje, layout: 'topRight', speed:300, timeout: 2000, type: 'information' });

        //        //$.sticky(Mensaje, { autoclose: 5000, position: "top-right", type: "st-error" });
        //        break;
        //    case 'Suceso':
        //        //$.sticky(Mensaje, { autoclose: 5000, position: "top-right", type: "st-success" });
        //        var a = noty({ text: Mensaje, layout: 'topRight', speed: 300, timeout: 2000, type: 'success' });
        //        break;
        //    case 'Info':
        //        //$.sticky(Mensaje, { autoclose: 5000, position: "top-right", type: "st-info" });
        //        var a = noty({ text: Mensaje, layout: 'topRight', speed: 300, timeout: 2000, type: 'information' });
        //        break;
        //    default:
        //        //$.sticky(Mensaje, { autoclose: 5000, position: "top-right" });
        //        var a = noty({ text: Mensaje, layout: 'topRight', speed: 300, timeout: 2000, type: 'information' });
        //}
        if (!Opcion) { Opcion = 'Normal'; }
        if (!Time) { Time = 3500; }

        switch (Opcion) {
            case 'Error':
                var a = noty({ text: Mensaje, layout: 'topRight', speed: 500, timeout: Time, type: 'error' });
                break;
            case 'Suceso':
                var a = noty({ text: Mensaje, layout: 'topRight', speed: 500, timeout: Time, type: 'success' });
                break;
            case 'Info':
                var a = noty({ text: Mensaje, layout: 'topRight', speed: 500, timeout: Time, type: 'information' });
                break;
            case 'Advertencia':
                var a = noty({ text: Mensaje, layout: 'topRight', speed: 500, timeout: Time, type: 'warning' });
                break;
            default:
                var a = noty({ text: Mensaje, layout: 'topRight', speed: 500, timeout: Time, type: 'information' });//notification
        }
    },
    //
    GetRowData: function (idJQGrid) {
        /// <summary>
        /// Obtiene el registro actual seleccionado de un jqgrid
        /// </summary>
        if ($('#' + idJQGrid).getGridParam('selrow') == null) {
            return null;
        }
        var id = $('#' + idJQGrid).getGridParam('selrow');
        var ret = $("#" + idJQGrid).getRowData(id);
        return ret;
    },
    //
    ValidaCargaArchivo: function (IdControl, TamanioMbMax, TiposArchivoValidos) {
        /// <summary>
        /// Realiza validaciones de tamaño máximo y extensiones permitidas al cargar un archivo en un input file.
        /// </summary>

        //valida extension
        var archivo = $("[id$=" + IdControl + "]").val();
        if (archivo == "") {
            PROYECTOMED.MostrarMensaje("No a seleccionado ningún archivo", "Advertencia");
            $("[id$=" + IdControl + "]").val('');
            return false;
        }
        var ext = archivo.substring(archivo.indexOf('.') + 1).toLowerCase();
        var ArchivoValido = false;
        for (var i = 0; i < TiposArchivoValidos.length; i++) {
            if (ext == TiposArchivoValidos[i]) {
                ArchivoValido = true;
                break;
            }
        }
        if (ArchivoValido == false) {
            PROYECTOMED.MostrarMensaje("La extensión del archivo no está permitido.", "Error");
            $("[id$=" + IdControl + "]").val('');
            return false;
        }

        //valida tamaño
        var sizeByte = $("[id$=" + IdControl + "]")[0].files[0].size;
        var sizeKiloByte = parseFloat(sizeByte / 1024);
        var sizeMegaByte = parseFloat(sizeKiloByte / 1024);

        if (sizeMegaByte > TamanioMbMax) {
            PROYECTOMED.MostrarMensaje("El tamaño supera el límite permitido de " + TamanioMbMax + "Mb.", "Error");
            $("[id$=" + IdControl + "]").val('');
            return false;
        }

        return true;
    },
    //
    MostrarPopup: function (Titulo, Mensaje, Opcion, CallbackAccept, CallbackCancel) {
        /// <summary>
        /// Muestra una ventana de dialogo
        /// </summary>
        /// <param name="Titulo" type="String">Titulo de la ventana de dialogo</param>
        /// <param name="Mensaje" type="String">Mensaje a mostrar</param>
        /// <param name="Opcion" type="String">(Opcional) indica el tipo de ventana a mostrar (Default = Aceptar)</param>
        /// <param name="CallbackAccept" type="function">(Opcional) Función a ejecutar luego de pulsar [Aceptar] ó [Si]</param>
        /// <param name="CallbackCancel" type="function">(Opcional) Función a ejecutar luego de pulsar [Cancelar], [No], ó al salir de la ventana</param>
        var executeCallBack = false;
        //
        if (!Opcion) { Opcion = 'Aceptar'; }
        //
        var dlg = '';
        // header
        dlg = dlg + '<div id="dlgBase" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">';
        dlg = dlg + '<div class="modal-dialog">';
        dlg = dlg + '<div class="modal-content">';
        dlg = dlg + '<div class="modal-header">';
        //dlg = dlg + '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
        dlg = dlg + '<h5 class="modal-title"><strong>' + Titulo + '</strong></h5>';
        dlg = dlg + '</div>';
        // body
        dlg = dlg + '<div class="modal-body"><p>' + Mensaje + '</p></div>';
        // footer
        dlg = dlg + '<div class="modal-footer">'
        if (Opcion == 'Aceptar') {
            dlg = dlg + '<a id="btnCgrPopup_Aceptar" class="btn btn-primary">Aceptar</a>';
        }
        if (Opcion == 'SiNo') {
            dlg = dlg + '<a id="btnCgrPopup_Si" class="btn btn-sm btn-primary">Si</a>';
            dlg = dlg + '<a id="btnCgrPopup_No" class="btn btn-sm btn-default">No</a>'
        }
        if (Opcion == 'AceptarCancelar') {
            dlg = dlg + '<a id="btnCgrPopup_Aceptar" class="btn btn-sm btn-primary">Aceptar</a>';
            dlg = dlg + '<a id="btnCgrPopup_Cancelar" class="btn btn-sm btn-default">Cancelar</a>'
        }
        dlg = dlg + '</div>';
        dlg = dlg + '</div>';
        dlg = dlg + '</div>';
        dlg = dlg + '</div>';

        //$("form").append(dlg);
        $("body").append(dlg);

        var dialogo = $("#dlgBase");

        /*
        aceptar
        */
        $('#btnCgrPopup_Aceptar, #btnCgrPopup_Si').click(function () {
            if (typeof CallbackAccept == 'function') {
                executeCallBack = true;
                dialogo.modal('hide');
                CallbackAccept.call(dialogo);
            } else {
                dialogo.modal('hide');
            }
            return true;
        });
        /*
        cancelar
        */
        $("#btnCgrPopup_Cancelar, #btnCgrPopup_No").click(function () {
            if (typeof CallbackCancel == 'function') {
                executeCallBack = true;
                dialogo.modal("hide");
                CallbackCancel.call(dialogo);
            } else {
                dialogo.modal("hide");
            }
            return true;
        });
        // cerrar ventana
        dialogo.on('hide.bs.modal', function () {
            switch (Opcion) {
                case 'Aceptar':
                    if (typeof CallbackAccept == 'function') {
                        if (!executeCallBack) { CallbackAccept.call(dialogo); }
                    }
                    break;
                case 'SiNo', 'AceptarCancelar':
                    if (typeof CallbackCancel == 'function') {
                        if (!executeCallBack) { CallbackCancel.call(dialogo); }
                    }
                    break;
                default:
                    break;
            }
            dialogo.remove();
        });
        /*
        muestra dialogo
        */

        //if ($.browser.msie) {
        //    $('#dlgBase').removeClass('fade');
        //}

        dialogo.modal({
            keyboard: false, backdrop: 'static'
        });

        //dialogo.modal('show');
    },
    //
    CargarGridLoad: function (strActionMethod, strGridId, objJsonParams, callbackOK, mappingAllModel) {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: strActionMethod,
            data: PROYECTOMED.SetParam(objJsonParams),
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert("some error");
            },
            complete: function (jsondata, stat) {
                //PROYECTOMED.HideProgress();
                var result = PROYECTOMED.GetResult(jsondata.responseJSON);

                if (stat == "success") {
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    //Se agregan las columnas que faltan
                    if (mappingAllModel) {
                        var colModelJson = result.Dato.colModel;
                        var colModelGrid = $("#" + strGridId).jqGrid('getGridParam', 'colModel');
                        var colNameGrid = $("#" + strGridId).jqGrid('getGridParam', 'colNames');
                        var caption = $("#" + strGridId).jqGrid('getGridParam', 'caption');

                        var colModelNew = [];
                        var colNamesNew = [];
                        var modelItem;
                        var modelName;
                        $.each(colModelJson, function (index, itemJson) {

                            modelItem = { name: itemJson, index: itemJson, hidden: true };
                            modelName = itemJson;
                            $.each(colModelGrid, function (index, modelGrid) {

                                if (itemJson == modelGrid.name) {
                                    modelItem = modelGrid;
                                    modelName = colNameGrid[index];
                                }
                            });


                            colModelNew.push(modelItem);
                            colNamesNew.push(modelName);


                        });

                        $('#' + strGridId).resguardeToolBar('toolBar_' + strGridId);//para resguardar el toolbar
                        $("#" + strGridId).jqGrid('GridUnload');
                        PROYECTOMED.ConfigJQGrid(strGridId, caption, null, colNamesNew, colModelNew);
                    }


                    $("#" + strGridId)[0].addJSONData(datTabla);
                    if (typeof callbackOK == 'function') { callbackOK.call(this); }

                    if (msgTabla)
                        PROYECTOMED.MostrarMensaje(msgTabla, 'success');
                }
                //else
                //PROYECTOMED.MostrarPopup("Error", "Error Cargar grilla.", "Aceptar", null, null, "Error"); // alert(JSON.parse(jsondata.responseText).Message);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            // alert('Error!!');
        });

    },
    //
    CargarGrid: function (strActionMethod, IdGrid, JsonParams, CallbackOk) {
        //CargarGrid/ <summary>
        /// Carga control Grid mediante método Ajax
        /// </summary>
        /// <param name="WebMethod" type="String">Nombre del WebMethod utilizado para cargar el Grid</param>
        /// <param name="IdGrid" type="String">Id del tag <Table> que se formatea como un Grid</param>
        /// <param name="JsonParams" type="Json">(Opcional) Parametros utilizados por el [WebMethod], serializados en notación Json</param>
        /// <param name="CallbackOk" type="function">(Opcional) Función a ejecutar después de realizar la carga del Grid</param>

        //PROYECTOMED.ShowProgress();

        $("#" + IdGrid).jqGrid('setGridParam', {
            datatype: function () {
                $.ajax({
                    url: strActionMethod,
                    data: JSON.stringify(JsonParams),
                    complete: function (jsondata, stat) {

                        //PROYECTOMED.HideProgress();

                        if (stat == "success") {
                            $("#" + IdGrid)[0].addJSONData(JSON.parse(jsondata.responseText));
                            if (typeof CallbackOk == 'function') { CallbackOk.call(this); }
                        }
                        //else
                        //PROYECTOMED.MostrarPopup("Error", "Error al cargar el Grid.", "Aceptar", null, null, "Error"); // alert(JSON.parse(jsondata.responseText).Message);
                    }
                });
            }
        }).trigger('reloadGrid');

    },
    //
    ConfigJQGrid: function (strGridId, strCaption, functionGetData, objJsonColumns, arrayModel) {
        /// <summary>
        /// Configura el control Grid
        /// </summary>
        /// <param name="IdGrid"            type="String">Id del tag <Table> que se formatea como un Grid</param>
        /// <param name="strCaption"        type="String">Caption del grid</param>
        /// <param name="functionGetData"   type="String"></param>
        /// <param name="objJsonColumns"    type="String"></param>
        /// <param name="arrayModel"        type="String"></param>
        $("#" + strGridId).jqGrid({
            datatype: functionGetData,
            colNames: objJsonColumns,
            colModel: arrayModel,
            rowNum: 20, rowList: [20, 40, 50], pager: '#pag_' + strGridId, sortname: 'cod', sortorder: 'asc',
            height: 'auto', shrinkToFit: true, autowidth: true, rownumbers: true, reloadAfterEdit: false, reloadAfterSubmit: false, viewrecords: true, toolbar: [true, 'top'], hidegrid: false, autoencode: true, caption: strCaption
        }).navGrid('#pag_' + strGridId, { refresh: false, search: false, add: false, edit: false, del: false });

        $('#' + strGridId).addToolBar('toolBar_' + strGridId);
        $('#cont_' + strGridId).resize(function () {
            $('#' + strGridId).jqGrid('setGridWidth', $('#cont_' + strGridId).width());
        });
        $('#' + strGridId)[0].grid.IsReady = true;
    },
    //
    JQGridForceResize: function (strGridId) {
        /// <summary>
        /// Fuerza el redimensionado del control Grid
        /// </summary>
        /// <param name="IdGrid"            type="String">Id del tag <Table> que se formatea como un Grid</param>
        $('#tbl' + strGridId).jqGrid('setGridWidth', $('#cnt' + strGridId).width());
    },
    //
    GetPaginationJQGrid: function (strGridId) {
        var parFiltro = new Object();
        parFiltro.TOKEN = PROYECTOMED.GetToken();
        parFiltro.Page_size = $('#' + strGridId).getGridParam('rowNum');
        parFiltro.Page_number = $('#' + strGridId).getGridParam('page');
        parFiltro.Sort_Column = $('#' + strGridId).getGridParam('sortname');
        parFiltro.Sort_Order = $('#' + strGridId).getGridParam('sortorder');
        return parFiltro;
    },
    //
    CargarCombo: function (strActionMethod, IdCombo, JsonParams, PrimerItem, CallbackOk) {
        /// <summary>
        /// Carga control Combobox mediante método Ajax
        /// </summary>
        /// <param name="strActionMethod" type="String">Nombre del WebMethod utilizado para cargar el Combo</param>
        /// <param name="IdCombo" type="String">Id del Combo</param>
        /// <param name="JsonParams" type="Json">(Opcional) Parametros utilizados por el [WebMethod], serializados en notación Json</param>
        /// <param name="PrimerItem" type="String">Texto que se agrega como primer elemento en el Combo, su value siempre es ['']</param>
        /// <param name="CallbackOk" type="function">(Opcional) Función a ejecutar después de realizar la carga del Combo</param>
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: strActionMethod,
            data: PROYECTOMED.SetParam(JsonParams),
            success: function (jsondata) {
                var result = PROYECTOMED.GetResult(jsondata);
                if (result.Lista != null) {
                    $("[id$=" + IdCombo + "]").html("");
                    if (PrimerItem != null) {
                        $("[id$=" + IdCombo + "]").append($("<option></option>")
                            .attr("value", "").text(PrimerItem));
                    }
                    $.each(result.Lista, function () {
                        $("[id$=" + IdCombo + "]").append($("<option></option>")
                            .attr("value", this.Id).text(this.Valor))
                    });
                    if (typeof CallbackOk == 'function') { CallbackOk.call(this); }
                }
            }
        });
    },
    //
    ClearCombo: function (IdCombo, PrimerItem) {
        $("[id$=" + IdCombo + "]").html("");
        if (PrimerItem != null) {
            $("[id$=" + IdCombo + "]").append($("<option></option>")
                .attr("value", "").text(PrimerItem));
        }
    },
    //
    GetObjectForm: function (strNameBase) {
        /// <summary>
        /// Obtiene un objeto conformado por propiedades que representan a los controles que empiezan con strNameBase
        /// Se obvian los prefijos de controles para tener un objeto limpio.
        /// </summary>
        var values = {};
        $("[id^='ddl" + strNameBase + "_']").each(function () {
            var customName1 = this.name.substring(4 + strNameBase.length, this.name.length).toUpperCase();
            values[customName1] = this.value;
        });
        $("[id^='txt" + strNameBase + "_']").each(function () {
            var customName2 = this.name.substring(4 + strNameBase.length, this.name.length).toUpperCase();
            values[customName2] = this.value;
        });
        return values;
    },
    SetObjectForm: function (strNameBase, objData) {
        /// <summary>
        /// establece un objeto de datos a una serie de controles que empiezan con strNameBase
        /// </summary>
        for (var property in objData) {
            var objFind;
            objFind = $("#dialogReg" + strNameBase).find("#ddl" + strNameBase + "_" + property);
            if (objFind.length > 0) {
                objFind.val(objData[property]);
            } else {
                objFind = $("#dialogReg" + strNameBase).find("#txt" + strNameBase + "_" + property);
                if (objFind.length > 0) {
                    $("#dialogReg" + strNameBase).find("#txt" + strNameBase + "_" + property).val(objData[property]);
                }
                //$("#txt" + strNameBase + "_" + property).val(objData[property]);
            }
        }
    },
    ConfiguraBoones: function (codigo_recurso, tool_bar, CallbackOk) {

        $.ajax({
            url: "MetodosGenericos.aspx/ConfigBoton",
            data: JSON.stringify({ 'token': '-', 'codRec': codigo_recurso }),
            success: function (result) {
                if (result.d.Dato != null) {
                    $("#" + tool_bar).html("");
                    $("#" + tool_bar).html(result.d.Dato);
                    if (typeof CallbackOk == 'function') { CallbackOk.call(this); }
                }
            }
        });

    },
    //
    CargarComboDesdeMetGeneric: function (WebMethod, IdCombo, JsonParams, PrimerItem, CallbackOk) {
        /// <summary>
        /// Carga control Combobox mediante método Ajax
        /// </summary>
        /// <param name="WebMethod" type="String">Nombre del WebMethod utilizado para cargar el Combo</param>
        /// <param name="IdCombo" type="String">Id del Combo</param>
        /// <param name="JsonParams" type="Json">(Opcional) Parametros utilizados por el [WebMethod], serializados en notación Json</param>
        /// <param name="PrimerItem" type="String">Texto que se agrega como primer elemento en el Combo, su value siempre es ['']</param>
        /// <param name="CallbackOk" type="function">(Opcional) Función a ejecutar después de realizar la carga del Combo</param>
        $.ajax({
            url: "MetodosGenericos.aspx/" + WebMethod,
            data: JSON.stringify(JsonParams),
            success: function (result) {
                if (result.d != null) {
                    $("[id$=" + IdCombo + "]").html("");
                    if (PrimerItem != null) {
                        $("[id$=" + IdCombo + "]").append($("<option></option>")
                            .attr("value", "").text(PrimerItem));
                    }
                    $.each(result.d, function () {
                        $("[id$=" + IdCombo + "]").append($("<option></option>")
                            .attr("value", this.Id).text(this.Valor))
                    });
                    if (typeof CallbackOk == 'function') { CallbackOk.call(this); }
                }
            }
        });
    },
    UrlActionMethod: function (Controller, Method) {
        /// <summary>
        /// Devuelve el Url de un WebMethod (con relación a la página donde se ejecuta el script)
        /// </summary>
        /// <param name="Controller" type="String">Nombre del Controller</param>
        /// <param name="Method" type="String">Nombre del Method</param>
        if ((Controller) && (Method)) {
            return "../" + Controller + "/" + Method;
        } else {
            return Controller;
        }
    },
    //
    GetToken: function () {
        //return $("[id$=" + "hfToken" + "]").val();
        return "";
    },
    GetEncryptClv: function () {
        return $("[name=" + "_hfencryptclv" + "]").val();
    },
    //
    IsEncrypt: function () {
        return ($("[name=" + "_hfencrypt" + "]").val() == 'S');
    },
    //
    SetParam: function (datParam) {
        if (PROYECTOMED.IsEncrypt()) {
            param = JSON.stringify({ filter: Encrypt(JSON.stringify(datParam), PROYECTOMED.GetEncryptClv()) });
        } else {
            param = JSON.stringify(datParam);
        }
        return param;
    },
    //
    GetResult: function (result) {
        if (result == undefined)
            return;

        if (PROYECTOMED.IsEncrypt()) {
            var decrypted = Decrypt(result, PROYECTOMED.GetEncryptClv());

            if (isJSON(decrypted)) {
                result = JSON.parse(decrypted);
            } else {
                result = decrypted;
            }

        }
        if (result.state != undefined) {
            if (!result.state) {
                if (result.statusCode == 999) {
                    //document.location.replace(PROYECTOMED.UrlActionMethod("Error"));
                    PROYECTOMED.ShowView(PROYECTOMED.UrlActionMethod("Error", "SesionExpirada"));
                }
            }
        }
        return result;
    },
    //
    ShowView: function (strActionMethod, objJsonParams) {
        var args = objJsonParams;
        if (PROYECTOMED.IsEncrypt() && objJsonParams != null) {
            args = { filter: Encrypt(JSON.stringify(objJsonParams), PROYECTOMED.GetEncryptClv()) };
        }

        var form = $("<form></form>");
        form.attr("method", "post");
        form.attr("action", strActionMethod);

        if (objJsonParams != null) {
            $.each(args, function (key, value) {
                var field = $('<input></input>');

                field.attr("type", "hidden");
                field.attr("name", key);
                field.attr("value", value);

                form.append(field);
            });
        }

        $(form).appendTo('body').submit();
        delete form;
    },
    //
    DownloadFile: function (strActionMethod, objJsonParams) {
        //Bloquear("Descargando archivo...");


        var form = $('<form>', { action: strActionMethod, method: 'post' });
        if (objJsonParams != null) {
            var args = objJsonParams;
            if (PROYECTOMED.IsEncrypt()) {
                args = { filter: Encrypt(JSON.stringify(objJsonParams), PROYECTOMED.GetEncryptClv()) };
            }
            $.each(args, function (key, value) {
                $(form).append($('<input>', { type: 'hidden', name: key, value: value }));
            });
        }
        $(form).appendTo('body');

        if (window.File && window.FileReader && window.FileList && window.Blob) {
            PROYECTOMED._DownloadFile($(form).attr('action'), $(form).serialize());
        } else {
            $(form).submit();
            // Desbloquear();
        }
        delete form;
    },
    //
    _DownloadFile: function (url, params) {

        var xhr = new XMLHttpRequest();
        xhr.responseType = 'blob';

        xhr.onload = function () {

            if (xhr.status === 200) {
                var filename = "";
                var disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                }

                var a = document.createElement('a');
                a.href = window.URL.createObjectURL(xhr.response); // xhr.response is a blob
                a.download = filename; // Set the file name.
                a.style.display = 'none';
                document.body.appendChild(a);
                a.click();
                delete a;
                //Desbloquear();
            } else {
                Desbloquear();
                PROYECTOMED.MostrarMensaje("Ocurrio un error al descargar el archivo, intente nuevamente.");
            }

        };
        xhr.onerror = function () {
            //Desbloquear();
            PROYECTOMED.MostrarMensaje("Ocurrio un error al descargar el archivo, intente nuevamente.");

        };

        //*******parameters*******//
        xhr.open('POST', url, true);
        xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xhr.send(params);
        //******************///

    },
    ToUpperCase: function (IdControl, permitirAlt) {
        /// <summary>
        /// Convierte a Mayúsculas el texto ingresado en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a aplicar</param>
        /// <param name="permitirAlt" type="Boolean">(Opcional) define si en el campo se puede presionar la lecla Alt</param>
        var objControl = $("[id$=" + IdControl + "]");

        if (permitirAlt == undefined) {
            permitirAlt = false;
        }

        objControl.keypress(function (e) {
            if ((e.charCode >= 97 & e.charCode <= 122) ||
                (e.charCode >= 65 & e.charCode <= 90) ||
                (e.charCode >= 48 & e.charCode <= 57) ||
                (e.charCode == 32) || (e.charCode == 45) ||
                (e.charCode == 209 || e.charCode == 241) ||
                (e.charCode == 44 || e.charCode == 34) ||  //(e.key == "ñ" || e.key == "Ñ") ||
                (e.charCode == 46) ||
                (e.charCode == 225 || e.charCode == 233 || e.charCode == 237 || e.charCode == 243 || e.charCode == 250) ||
                (e.charCode == 193 || e.charCode == 201 || e.charCode == 205 || e.charCode == 211 || e.charCode == 218) ||
                (e.charCode == 228 || e.charCode == 235 || e.charCode == 239 || e.charCode == 246 || e.charCode == 252) ||
                (e.charCode == 196 || e.charCode == 203 || e.charCode == 207 || e.charCode == 214 || e.charCode == 220)) {
                if (objControl.val().length >= objControl.attr("maxLength")) {
                    return false;
                }
                var pst = e.currentTarget.selectionStart;
                var pst2 = e.currentTarget.selectionEnd;
                var string_start = e.currentTarget.value.substring(0, pst);
                var string_end = e.currentTarget.value.substring(pst2, e.currentTarget.value.length);
                e.currentTarget.value = string_start + String.fromCharCode(e.charCode).toUpperCase() + string_end;
                objControl.val(e.currentTarget.value);
                e.currentTarget.selectionStart = pst + 1;
                e.currentTarget.selectionEnd = pst + 1;

                return false;
            }
            else if (e.key != "/" && e.key != "!" && e.key != "¡" && e.key != "¿" && e.key != "?" &&
                e.key != "@" && e.key != "Backspace" && e.key != "Delete" && e.key != "ArrowRight" &&
                e.key != "ArrowLeft" && e.key != '"' && e.key != ' ' && e.key != '°' && e.key != ',' &&
                e.key != "'" && e.key != '-' && e.key != 'Home' && e.key != 'End') {
                return false;
            }
            else if (e.key == "Alt" && e.charCode == 64) {
                return false;
            }
            else if (e.key == "Alt" && permitirAlt == false) {
                return false;
            }
        });
    },
    //
    ToUpperCaseEmail: function (IdControl) {
        /// <summary>
        /// Convierte a Mayúsculas el texto ingresado en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a aplicar</param>
        /// <param name="permitirAlt" type="Boolean">(Opcional) define si en el campo se puede presionar la lecla Alt</param>
        var objControl = $("[id$=" + IdControl + "]");

        objControl.keypress(function (e) {
            if ((e.charCode >= 97 & e.charCode <= 122) ||
                (e.charCode >= 65 & e.charCode <= 90)) {
                if (objControl.val().length >= objControl.attr("maxLength")) {
                    return false;
                }
                var pst = e.currentTarget.selectionStart;
                var string_start = e.currentTarget.value.substring(0, pst);
                var string_end = e.currentTarget.value.substring(pst, e.currentTarget.value.length);
                e.currentTarget.value = string_start + String.fromCharCode(e.charCode).toUpperCase() + string_end;
                objControl.val(e.currentTarget.value);
                e.currentTarget.selectionStart = pst + 1;
                e.currentTarget.selectionEnd = pst + 1;

                return false;
            }
        });
    },
    //
    CheckEmail: function (IdControl, Mensaje) {
        /// <summary>
        /// Valida que el control contenga un email
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a validar</param>
        /// <param name="Mensaje" type="String">Mensaje mostrado en caso se validación sea incorrecta</param>

        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        valControl = objControl.val();

        if (valControl.trim() != '') {
            expr = /^([a-zA-Z0-9_\.\-\+\[\]])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!expr.test(valControl)) {
                objControl.addClass("ui-state-error");
                PROYECTOMED.MostrarMensaje(Mensaje, 'Error');
                objControl.focus();
                return false;
            } else {
                objControl.removeClass("ui-state-error");
                return true;
            }
        } else {
            objControl.removeClass("ui-state-error");
            return true;
        }
    },
    //
    CheckLength: function (IdControl, Mensaje, Min, Max, OcultarMensaje, IdControlMensaje, Mask) {
        /// <summary>
        /// Valida la longitud de un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a validar</param>
        /// <param name="Mensaje" type="String">Mensaje mostrado en caso se validación sea incorrecta</param>
        /// <param name="Min" type="Number">Longitud mínima requerida para el control</param>
        /// <param name="Max" type="Number">Longitud máxima requerida para el control</param>
        /// <param name="OcultarMensaje" type="Boolean">(Opcional) Oculta mensaje de validación</param>
        /// <param name="IdControlMensaje" type="String">(Opcional) Id del control que muestra el mensaje de validación</param>
        /// <param name="Mask" type="String">Máscara empleada por el control (la evita este caracter para realizar una verificacion real)</param>

        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        var objControlMsg = $("[id$=" + IdControlMensaje + "]");

        if (Mask) {
            var cad = '';
            for (var x = 0; x < objControl.val().length; x++) {
                if (objControl.val().charAt(x) != Mask)
                    cad = cad + objControl.val().charAt(x);
            }
            valControl = cad.length;
        } else
            valControl = objControl.val().length;

        if (valControl > Max || valControl < Min) {
            objControl.addClass("ui-state-error");
            if (!OcultarMensaje) {
                if (!IdControlMensaje) {
                    PROYECTOMED.MostrarMensaje(Mensaje, 'Error');
                    objControl.focus();
                }
                else {
                    $("[id$=" + IdControlMensaje + "] > msg").text('');
                    $("[id$=" + IdControlMensaje + "] > msg").append(Mensaje);
                    objControlMsg.show();
                    objControl.focus();
                }
            }
            return false;
        } else {
            objControl.removeClass("ui-state-error");
            if (IdControlMensaje) {
                $("[id$=" + IdControlMensaje + "] > msg").text('');
                objControlMsg.hide();
            }
            return true;
        }
    },
    //
    CheckVal: function (IdControl, Mensaje, Min, Max, OcultarMensaje) {
        /// <summary>
        /// Valida el rango de valores permitidos en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a validar</param>
        /// <param name="Mensaje" type="String">Mensaje mostrado en caso se validación sea incorrecta</param>
        /// <param name="Min" type="Number">Valor mínimo permitido para el control</param>
        /// <param name="Max" type="Number">Valor máximo permitido para el control</param>
        /// <param name="OcultarMensaje" type="Boolean">(Opcional) Oculta mensaje de validación</param>

        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        valControl = objControl.val();

        if (valControl > Max || valControl < Min) {
            objControl.addClass("ui-state-error");
            if (!OcultarMensaje) {
                //PROYECTOMED.MostrarPopup('Error', Mensaje, 'Aceptar', function () { objControl.focus(); }, null, "Error");
                PROYECTOMED.MostrarMensaje(Mensaje, 'Error');
                objControl.focus();
            }
            return false;
        } else {
            objControl.removeClass("ui-state-error");
            return true;
        }
    },
    //
    CheckDate: function (IdControl, Mensaje, OcultarMensaje) {
        /// <summary>
        /// Valida si el control proporcionado contiene un dato de fecha válido
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a validar</param>
        /// <param name="Mensaje" type="String">Mensaje mostrado en caso se validación sea incorrecta</param>
        /// <param name="OcultarMensaje" type="Boolean">(Opcional) Oculta mensaje de validación</param>

        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        valControl = objControl.val();

        if (!PROYECTOMED.IsDate(valControl)) {
            objControl.addClass("ui-state-error");
            if (!OcultarMensaje) {
                PROYECTOMED.MostrarMensaje(Mensaje, 'Error');
            }
            return false;
        } else {
            objControl.removeClass("ui-state-error");
        }

        return true;
    },
    //
    CheckRangeDate: function (IdControlIni, IdControlFin, Mensaje, OcultarMensaje) {
        /// <summary>
        /// Valida si el control proporcionado contiene un dato de fecha válido
        /// </summary>
        /// <param name="IdControlIni" type="String">Id del control con la fecha de inicio a validar</param>
        /// <param name="IdControlFin" type="String">Id del control con la fecha de fin a validar</param>
        /// <param name="Mensaje" type="String">Mensaje mostrado en caso se validación sea incorrecta</param>
        /// <param name="OcultarMensaje" type="Boolean">(Opcional) Oculta mensaje de validación</param>

        //valida fechas individualmente
        for (var i = 1; i < 3; i++) {
            var IdControl = (i = 1 ? IdControlIni : IdControlFin);
            var TxControl = (i = 1 ? 'Inicio' : 'Término');

            var valControl;
            var objControl = $("[id$=" + IdControl + "]");
            if (!objControl)
                return false;

            valControl = objControl.val();

            if (!PROYECTOMED.IsDate(valControl)) {
                objControl.addClass("ui-state-error");
                if (!OcultarMensaje) {
                    PROYECTOMED.MostrarMensaje('Verifique la fecha de ' + TxControl + ' </br>[Formato dd/mm/yyyy]', 'Error');
                }
                return false;
            } else {
                objControl.removeClass("ui-state-error");
            }
        }
        //valida rango
        if (PROYECTOMED.StringToDate($("[id$=" + IdControlIni + "]").val()) > PROYECTOMED.StringToDate($("[id$=" + IdControlFin + "]").val())) {
            $("[id$=" + IdControlIni + "]").addClass("ui-state-error");
            $("[id$=" + IdControlFin + "]").addClass("ui-state-error");
            PROYECTOMED.MostrarMensaje(Mensaje, 'Error');
            return false;
        } else {
            $("[id$=" + IdControlIni + "]").removeClass("ui-state-error");
            $("[id$=" + IdControlFin + "]").removeClass("ui-state-error");
        }

        return true;
    },
    //
    IsDate: function (txtDate) {
        /// <summary>
        /// Valida si el texto proporcionado es una fecha
        /// </summary>
        /// <param name="txtDate" type="String">cadena que representa una fecha</param>
        var currVal = txtDate;
        if (currVal == '')
            return false;

        //Declare Regex  
        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var dtArray = currVal.match(rxDatePattern); // is format OK?

        if (dtArray == null)
            return false;

        //Checks for dd/mm/yyyy format.
        var dtDay = dtArray[1];
        var dtMonth = dtArray[3];
        var dtYear = dtArray[5];

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {
            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
            if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }
        if (dtYear < 1900 || dtYear > 2500)
            return false;
        return true;
    },
    ////
    //StringToDate: function (str) {
    //    var dia = str.substr(0, 2);
    //    var mes = str.substr(3, 2) - 1;
    //    var anho = str.substr(6, 4);

    //    return new Date(anho, mes, dia);
    //},
    //
    StringToDate: function (str) {
        var dateArray = str.split(/[\.|\/|-]/);
        return new Date(dateArray[2], dateArray[1] - 1, dateArray[0]);
    },


    //
    isValidDate: function (IdControl, Mensaje, OcultarMensaje) {
        /// <summary>
        /// Valida que el contenido del control sea una fecha valida en formato DD/MM/YYYY
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a validar</param>
        /// <param name="Mensaje" type="String">Mensaje mostrado en caso se validación sea incorrecta</param>
        /// <param name="OcultarMensaje" type="Boolean">(Opcional) Oculta mensaje de validación</param>
        var valControl;
        var objControl = $("[id$=" + IdControl + "]");
        if (!objControl)
            return false;

        var s = objControl.val();

        if (s.length != 10) {
            objControl.addClass("ui-state-error");
            if (!OcultarMensaje) {
                //PROYECTOMED.MostrarPopup('Error', Mensaje, 'Aceptar', function () { objControl.focus(); }, null, "Error");
                PROYECTOMED.MostrarMensaje(Mensaje, "Error");
                objControl.focus();
            }
            return false;
        }

        // format D(D)/M(M)/(YY)YY
        var dateFormat = /^\d{1,4}[\.|\/|-]\d{1,2}[\.|\/|-]\d{1,4}$/;
        if (dateFormat.test(s)) {
            // remover ceros del supuesto valor fecha
            s = s.replace(/0*(\d*)/gi, "$1");
            var dateArray = s.split(/[\.|\/|-]/);
            // valor de mes correcto
            dateArray[1] = dateArray[1] - 1;
            // valor de año correcto
            if (dateArray[2].length < 4) {
                // valor de año correcto
                dateArray[2] = (parseInt(dateArray[2]) < 50) ? 2000 + parseInt(dateArray[2]) : 1900 + parseInt(dateArray[2]);
            }
            var testDate = new Date(dateArray[2], dateArray[1], dateArray[0]);
            if (testDate.getDate() != dateArray[0] || testDate.getMonth() != dateArray[1] || testDate.getFullYear() != dateArray[2]) {
                objControl.addClass("ui-state-error");
                if (!OcultarMensaje) {
                    //PROYECTOMED.MostrarPopup('Error', Mensaje, 'Aceptar', function () { objControl.focus(); }, null, "Error");
                    PROYECTOMED.MostrarMensaje(Mensaje, "Error");
                    objControl.focus();
                }
                return false;
            } else {
                objControl.removeClass("ui-state-error");
                return true;
            }
        } else {
            objControl.addClass("ui-state-error");
            if (!OcultarMensaje) {
                //PROYECTOMED.MostrarPopup('Error', Mensaje, 'Aceptar', function () { objControl.focus(); }, null, "Error");
                PROYECTOMED.MostrarMensaje(Mensaje, "Error");
                objControl.focus();
            }
            return false;
        }
    },
    //
    ToUpperCaseSinEspaciosDobles: function (IdControl, permitirAlt) {
        /// <summary>
        /// Convierte a Mayúsculas el texto ingresado en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a aplicar</param>
        /// <param name="permitirAlt" type="Boolean">(Opcional) define si en el campo se puede presionar la lecla Alt</param>
        var objControl = $("[id$=" + IdControl + "]");

        if (permitirAlt == undefined) {
            permitirAlt = false;
        }

        objControl.keypress(function (e) {

            /*******************VERIFICA SI TIENE ESPACIO DOBLES******************************/
            var key = e.keyCode || e.which;
            var tecla = String.fromCharCode(key).toLowerCase();
            //var value = e.target.value + "" + tecla;

            var _pst = e.currentTarget.selectionStart;
            var _pst2 = e.currentTarget.selectionEnd;
            var _string_start = e.currentTarget.value.substring(0, _pst);
            var _string_end = e.currentTarget.value.substring(_pst2, e.currentTarget.value.length);
            value = _string_start + tecla + _string_end



            var existe = ExisteDobleEspacio(value);
            if (existe) {
                return false;
            }
            /*********************************************************************************/

            if ((e.charCode >= 97 & e.charCode <= 122) ||
                (e.charCode >= 65 & e.charCode <= 90) ||
                (e.charCode >= 48 & e.charCode <= 57) ||
                (e.charCode == 32) || (e.charCode == 45) ||
                (e.charCode == 209 || e.charCode == 241) ||
                (e.charCode == 44 || e.charCode == 34) ||  //(e.key == "ñ" || e.key == "Ñ") ||
                (e.charCode == 46) ||
                (e.charCode == 225 || e.charCode == 233 || e.charCode == 237 || e.charCode == 243 || e.charCode == 250) ||
                (e.charCode == 193 || e.charCode == 201 || e.charCode == 205 || e.charCode == 211 || e.charCode == 218) ||
                (e.charCode == 228 || e.charCode == 235 || e.charCode == 239 || e.charCode == 246 || e.charCode == 252) ||
                (e.charCode == 196 || e.charCode == 203 || e.charCode == 207 || e.charCode == 214 || e.charCode == 220)) {
                if (objControl.val().length >= objControl.attr("maxLength")) {
                    return false;
                }
                var pst = e.currentTarget.selectionStart;
                var pst2 = e.currentTarget.selectionEnd;
                var string_start = e.currentTarget.value.substring(0, pst);
                var string_end = e.currentTarget.value.substring(pst2, e.currentTarget.value.length);
                e.currentTarget.value = string_start + String.fromCharCode(e.charCode).toUpperCase() + string_end;
                objControl.val(e.currentTarget.value);
                e.currentTarget.selectionStart = pst + 1;
                e.currentTarget.selectionEnd = pst + 1;

                return false;
            }
            else if (e.key != "%" && e.key != "°" && e.key != "/" && e.key != "!" && e.key != "¡" && e.key != "¿" && e.key != "?" && e.key != "(" && e.key != ")" &&
                e.key != "@" && e.key != "Backspace" && e.key != "Delete" && e.key != "ArrowRight" &&
                e.key != "ArrowLeft" && e.key != '"' && e.key != ' ' && e.key != ',' && e.key != ':' &&
                e.key != '-' && e.key != '_' && e.key != 'Home' && e.key != 'End') {
                return false;
            }
            else if (e.key == "Alt" && e.charCode == 64) {
                return false;
            }
            else if (e.key == "Alt" && permitirAlt == false) {
                return false;
            }
        });
    },
    //
    ToPasswordSinEspaciosDobles: function (IdControl, permitirAlt) {
        /// <summary>
        /// Convierte a Mayúsculas el texto ingresado en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a aplicar</param>
        /// <param name="permitirAlt" type="Boolean">(Opcional) define si en el campo se puede presionar la lecla Alt</param>
        var objControl = $("[id$=" + IdControl + "]");

        if (permitirAlt == undefined) {
            permitirAlt = false;
        }

        objControl.keypress(function (e) {

            /*******************VERIFICA SI TIENE ESPACIO DOBLES******************************/
            var key = e.keyCode || e.which;
            var tecla = String.fromCharCode(key);
            //var value = e.target.value + "" + tecla;

            var _pst = e.currentTarget.selectionStart;
            var _pst2 = e.currentTarget.selectionEnd;
            var _string_start = e.currentTarget.value.substring(0, _pst);
            var _string_end = e.currentTarget.value.substring(_pst2, e.currentTarget.value.length);
            value = _string_start + tecla + _string_end



            var existe = ExisteDobleEspacio(value);
            if (existe) {
                return false;
            }
            /*********************************************************************************/

            if ((e.charCode >= 97 & e.charCode <= 122) ||
                (e.charCode >= 65 & e.charCode <= 90) ||
                (e.charCode >= 48 & e.charCode <= 57) ||
                (e.charCode == 32) || (e.charCode == 45) ||
                (e.charCode == 209 || e.charCode == 241) ||
                (e.charCode == 44 || e.charCode == 34) ||  //(e.key == "ñ" || e.key == "Ñ") ||
                (e.charCode == 46) ||
                (e.charCode == 225 || e.charCode == 233 || e.charCode == 237 || e.charCode == 243 || e.charCode == 250) ||
                (e.charCode == 193 || e.charCode == 201 || e.charCode == 205 || e.charCode == 211 || e.charCode == 218) ||
                (e.charCode == 228 || e.charCode == 235 || e.charCode == 239 || e.charCode == 246 || e.charCode == 252) ||
                (e.charCode == 196 || e.charCode == 203 || e.charCode == 207 || e.charCode == 214 || e.charCode == 220)) {
                if (objControl.val().length >= objControl.attr("maxLength")) {
                    return false;
                }
                var pst = e.currentTarget.selectionStart;
                var pst2 = e.currentTarget.selectionEnd;
                var string_start = e.currentTarget.value.substring(0, pst);
                var string_end = e.currentTarget.value.substring(pst2, e.currentTarget.value.length);
                e.currentTarget.value = string_start + String.fromCharCode(e.charCode) + string_end;
                objControl.val(e.currentTarget.value);
                e.currentTarget.selectionStart = pst + 1;
                e.currentTarget.selectionEnd = pst + 1;

                return false;
            }
            else if (e.key != "%" && e.key != "°" && e.key != "/" && e.key != "!" && e.key != "¡" && e.key != "¿" && e.key != "?" && e.key != "(" && e.key != ")" &&
                e.key != "@" && e.key != "Backspace" && e.key != "Delete" && e.key != "ArrowRight" &&
                e.key != "ArrowLeft" && e.key != '"' && e.key != ' ' && e.key != ',' && e.key != ':' &&
                e.key != '#' && e.key != '$' && e.key != '&' && e.key != '*' && e.key != ';' && e.key != '{' && e.key != '}' && e.key != '[' && e.key != ']' &&
                e.key != '-' && e.key != '_' && e.key != 'Home' && e.key != 'End') {
                return false;
            }
            else if (e.key == "Alt" && e.charCode == 64) {
                return false;
            }
            else if (e.key == "Alt" && permitirAlt == false) {
                return false;
            }
        });
    },
    //
    ToUpperCaseSinEspaciosDoblesEmail: function (IdControl, permitirAlt) {
        /// <summary>
        /// Convierte a Mayúsculas el texto ingresado en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a aplicar</param>
        /// <param name="permitirAlt" type="Boolean">(Opcional) define si en el campo se puede presionar la lecla Alt</param>
        var objControl = $("[id$=" + IdControl + "]");

        if (permitirAlt == undefined) {
            permitirAlt = false;
        }

        objControl.keypress(function (e) {

            /*******************VERIFICA SI TIENE ESPACIO DOBLES******************************/
            var key = e.keyCode || e.which;
            var tecla = String.fromCharCode(key).toLowerCase();
            //var value = e.target.value + "" + tecla;

            var _pst = e.currentTarget.selectionStart;
            var _pst2 = e.currentTarget.selectionEnd;
            var _string_start = e.currentTarget.value.substring(0, _pst);
            var _string_end = e.currentTarget.value.substring(_pst2, e.currentTarget.value.length);
            value = _string_start + tecla + _string_end



            var existe = ExisteDobleEspacio(value);
            if (existe) {
                return false;
            }
            /*********************************************************************************/

            if ((e.charCode >= 97 & e.charCode <= 122) ||
                (e.charCode >= 65 & e.charCode <= 90) ||
                (e.charCode >= 48 & e.charCode <= 57) ||
                (e.charCode == 32) || (e.charCode == 45) ||
                (e.charCode == 209 || e.charCode == 241) ||
                (e.charCode == 44 || e.charCode == 34) ||  //(e.key == "ñ" || e.key == "Ñ") ||
                (e.charCode == 46) ||
                (e.charCode == 225 || e.charCode == 233 || e.charCode == 237 || e.charCode == 243 || e.charCode == 250) ||
                (e.charCode == 193 || e.charCode == 201 || e.charCode == 205 || e.charCode == 211 || e.charCode == 218) ||
                (e.charCode == 228 || e.charCode == 235 || e.charCode == 239 || e.charCode == 246 || e.charCode == 252) ||
                (e.charCode == 196 || e.charCode == 203 || e.charCode == 207 || e.charCode == 214 || e.charCode == 220)) {
                if (objControl.val().length >= objControl.attr("maxLength")) {
                    return false;
                }
                var pst = e.currentTarget.selectionStart;
                var pst2 = e.currentTarget.selectionEnd;
                var string_start = e.currentTarget.value.substring(0, pst);
                var string_end = e.currentTarget.value.substring(pst2, e.currentTarget.value.length);
                e.currentTarget.value = string_start + String.fromCharCode(e.charCode).toUpperCase() + string_end;
                objControl.val(e.currentTarget.value);
                e.currentTarget.selectionStart = pst + 1;
                e.currentTarget.selectionEnd = pst + 1;

                return false;
            }
            else if (e.key != "%" && e.key != "°" && e.key != "/" && e.key != "!" && e.key != "¡" && e.key != "¿" && e.key != "?" && e.key != "(" && e.key != ")" &&
                e.key != "@" && e.key != "Backspace" && e.key != "Delete" && e.key != "ArrowRight" &&
                e.key != "ArrowLeft" && e.key != '"' && e.key != ' ' && e.key != ',' && e.key != '[' && e.key != ']' && e.key != '+' &&
                e.key != '-' && e.key != '_' && e.key != 'Home' && e.key != 'End') {
                return false;
            }
            else if (e.key == "Alt" && e.charCode == 64) {
                return false;
            }
            else if (e.key == "Alt" && permitirAlt == false) {
                return false;
            }
        });
    },
    //
    ShowProgress: function () {
        $("#DEV_PleaseWaitDialog").modal("show");
    },
    //
    HideProgress: function () {
        //$("#DEV_PleaseWaitDialog").modal("hide",true);
        $("#DEV_PleaseWaitDialog").modal("hide");
    },
    //
    //SetFormatPicker: function (IdControl, IddpControl, CallbackOk, conMask) {
    //    if (conMask)
    //        $("[id$=" + IdControl + "]").inputmask('99/99/9999');

    //    $("[id$=" + IdControl + "]").datepicker({
    //        language: 'es', format: 'dd/mm/yyyy', autoclose: true
    //    }).on("focusout", function (ev) {
    //        if (ev.currentTarget.value == '__/__/____') {
    //            $('#' + IdControl).val('');
    //        }
    //    })
    //    .bind("changeDate", function (e) {
    //        if (e.viewMode == 'days') {
    //            setTimeout(function () {
    //                $("[id$=" + IdControl + "]").datepicker("hide");
    //            }, 0);
    //            if (typeof CallbackOk == 'function') { CallbackOk.call(this); }
    //        }
    //    })
    //    .on("click", function () {
    //        setTimeout(function () {
    //            $("[id$=" + IdControl + "]").datepicker("hide");
    //        }, 0);
    //    });

    //    $('#' + IddpControl).click(function () {
    //        $("[id$=" + IdControl + "]").datepicker('show');
    //    });
    //},
    SetFormatPicker: function (IdGrupoControl) {
        //$('#txt' + IdGrupoControl.substring(3, IdGrupoControl.length - 2)).inputmask({ mask: "99/99/9999", placeholder: "jaime" });

        $('#' + IdGrupoControl + ' .input-group.date').datepicker({
            format: 'dd/mm/yyyy',
            language: "es",
            todayBtn: "linked",
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: true,
            autoclose: true,
        }).on('show', function () {
            if ($('#' + IdGrupoControl).attr('readonly')) {
                $(this).datepicker('hide');
            }
        });


    },
    //
    SetFormatPickerDisabled: function (IdGrupoControl, disabled) {
        var ind = (disabled ? 'disabled' : '');
        $('#' + IdGrupoControl + ' .form-control').prop('disabled', ind);

        if (ind == 'disabled') {
            $("#" + IdGrupoControl).attr('readonly', 'readonly');
        } else {
            $("#" + IdGrupoControl).removeAttr('readonly');
        }
    },
    SetValFechas: function (FechaIni, FechaFin) {

        var dia1 = parseFloat(FechaIni.substr(0, 2));
        var dia2 = parseFloat(FechaFin.substr(0, 2));
        var mes1 = parseFloat(FechaIni.substr(3, 2));
        var mes2 = parseFloat(FechaFin.substr(3, 2));
        var anio1 = parseFloat(FechaIni.substr(6, 4));
        var anio2 = parseFloat(FechaFin.substr(6, 4));


        var mismoAnio = (anio1 == anio2);
        var mismoMes = (mes1 == mes2);

        if (mismoAnio) {

            if (mismoMes) {

                if (dia2 > dia1) {

                    return true;
                } else {

                    return false;
                }

            } else {
                if (mes2 > mes1) {

                    return true;

                } else {

                    return false;
                }

            }


        } else {
            if (anio2 > anio1) {
                return true;

            } else { return false; }

        }



    },
    //
    EmailCorrecto: function (texto_email) {
        /// <summary>
        /// Valida que el texto sea un email
        /// </summary>
        /// <param name="texto_email" type="String">Texto a validar</param>

        var valControl;
        valControl = texto_email;
        if (valControl.trim() != '') {
            expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!expr.test(valControl)) {
                return false;
            } else {
                return true;
            }
        } else {
            return true;
        }
    },
    //
    Right: function (str, n) {
        if (n <= 0)
            return "";
        else if (n > String(str).length)
            return str;
        else {
            var iLen = String(str).length;
            return String(str).substring(iLen, iLen - n);
        }
    },
    //
    changeContainer: function (idcontainer, idelemento) {
        $("#" + idelemento).appendTo("#" + idcontainer);
    },
    //
    ConfigTab: function (tab, opcion) {

        //for (var i = 1; i < 10; i++) {

        //    if (i == tab) {
        //        $("#tab0" + tab).show();
        //        $('#ribbon-tab-header-0' + tab).addClass('file');

        //    } else {
        //        $("#tab0" + i).hide();
        //        $('#ribbon-tab-header-0' + i).removeClass('file');
        //    }
        //}

        //var tabs = ["0100", "0200", "0300", "0400", "0500", "0600", "0700", "0800"];
        //$(".ribbon-button").removeClass("ribbon-button-seleccionado");

        //$(".ribbon-button").show();
        //$(".ribbon-container-content").hide();
        //$("[id*=ribbon-tab-header-]").removeClass('file');
        //$("#tab" + tab).show();
        //$('#ribbon-tab-header-' + tab).addClass('file');

        //jQuery.each(tabs, function (i, val) {
        //    if (val == tab) {
        //        $("#tab" + tab).show();
        //        $('#ribbon-tab-header-' + tab).addClass('file');
        //    } else {
        //        $("#tab" + val).hide();
        //        $('#ribbon-tab-header-' + val).removeClass('file');
        //    }
        //});

        //$("#" + opcion).addClass("ribbon-button-seleccionado");



        var comprobar = $(".metismenu #" + tab).next();
        $('.metismenu > ul > li').removeClass('active');
        $(".metismenu #" + tab).closest('li').addClass('active');
        if ((comprobar.is('ul')) && (comprobar.is(':visible'))) {
            $(".metismenu #" + tab).closest('li').removeClass('active');
            comprobar.slideUp('normal');
        }
        if ((comprobar.is('ul')) && (!comprobar.is(':visible'))) {
            $('.metismenu ul ul:visible').slideUp('normal');
            comprobar.slideDown('normal');
        }

        $('.metismenu ul li #' + opcion).css("background-color", "#424242");
        $('.metismenu ul li #' + opcion).css("cursor", "pointer");
        $('.metismenu ul li #' + opcion).css("border-left", "4px solid red");
        $('.metismenu ul li #' + opcion).css("border-radius", "5px");


    },
    //
    Ajax: function (option) {

        option.applicatioPath = option.applicatioPath == undefined ? "" : option.applicatioPath; //CONF.PARAMETROS.APPLICATION_PATH : option.applicatioPath;
        option.progress = option.progress == undefined ? false : option.progress;
        option.type = option.type == undefined ? 'POST' : option.type;
        option.async = option.async == undefined ? true : option.async;
        option.dataType = option.dataType == undefined ? 'json' : option.dataType;
        option.contentType = option.contentType == undefined ? 'application/json; charset=utf-8' : option.contentType;
        option.tokenRequired = option.tokenRequired == undefined ? false : option.tokenRequired;


        $.ajax({
            type: option.type,
            url: option.applicatioPath + option.url,
            async: option.async,
            dataType: option.dataType,
            contentType: option.contentType,
            data: option.data,
            beforeSend: function (xhr) {
                if (option.progress) {
                    Bloquear(option.progressText);
                }

                if (option.tokenRequired) {

                    $.ajax({
                        type: "POST",
                        async: false,
                        url: PROYECTOMED.UrlActionMethod("Access", "KeyToken"),// option.applicatioPath + "../Access/KeyToken",
                        contentType: 'application/json; charset=utf-8',
                        success: function (result) {

                            result = PROYECTOMED.GetResult(result);
                            var datToken = result.Dato;
                            var msgToken = result.Msg;
                            if (datToken) {

                                if (PROYECTOMED.IsEncrypt()) {
                                    datToken = Encrypt(datToken, PROYECTOMED.GetEncryptClv());
                                }

                                xhr.setRequestHeader("Authorization", "Bearer " + datToken);
                            } else {
                                PROYECTOMED.MostrarMensaje(msgToken, 'Error');
                            }



                        }

                    });
                }
            },
            error: function (xhr) {

                PROYECTOMED.MostrarMensaje(xhr.responseJSON, 'Error');
            },
            success: function (result) {

                result = PROYECTOMED.GetResult(result);


                if (result.state == undefined) {

                    //var datResponse = result.Dato;
                    //var msgResponse = result.Msg;
                    //if (datResponse) {
                    option.success(result);
                    //} else {
                    //PROYECTOMED.MostrarMensaje(msgResponse, 'Error');
                    //}

                } else {
                    if (!result.state) {

                        if (result.statusCode === 401) {
                            PROYECTOMED.MostrarMensaje("Acceso no autorizado", 'Error');
                        }

                        if (result.statusCode === 404) {
                            PROYECTOMED.MostrarMensaje("Página no encontrada", 'Error');
                        }

                        if (result.statusCode === 405) {
                            PROYECTOMED.MostrarMensaje("Acceso no autorizado", 'Error');
                        }

                    }
                }
            },
            complete: function () {
                if (option.progress) {
                    Desbloquear();
                }
            }
        });
    },

    AjaxUpload: function (option) {

        option.applicatioPath = option.applicatioPath == undefined ? "" : option.applicatioPath; //CONF.PARAMETROS.APPLICATION_PATH : option.applicatioPath;
        option.progress = option.progress == undefined ? false : option.progress;


        $.ajax({
            type: "POST",
            url: option.applicatioPath + option.url,
            contentType: false,
            processData: false,
            //async: option.async,
            dataType: "json",//option.dataType,
            //contentType: option.contentType,
            data: option.data,
            beforeSend: function (xhr) {
                if (option.progress) {
                    Bloquear(option.progressText);
                }

                //Solicitamos el token
                $.ajax({
                    type: "POST",
                    async: false,
                    url: PROYECTOMED.UrlActionMethod("Access", "KeyToken"),//option.applicatioPath + "/Access/KeyToken",
                    contentType: 'application/json; charset=utf-8',
                    success: function (result) {

                        result = PROYECTOMED.GetResult(result);
                        var datToken = result.Dato;
                        var msgToken = result.Msg;
                        if (datToken) {

                            if (PROYECTOMED.IsEncrypt()) {
                                datToken = Encrypt(datToken, PROYECTOMED.GetEncryptClv());
                            }

                            xhr.setRequestHeader("Authorization", "Bearer " + datToken);
                        } else {
                            PROYECTOMED.MostrarMensaje(msgToken, 'Error');
                        }



                    }

                });
            },
            error: function (xhr) {

                PROYECTOMED.MostrarMensaje(xhr.responseText, 'Error');
            },
            success: function (result) {

                result = PROYECTOMED.GetResult(result);


                if (result.state == undefined) {

                    var datResponse = result.Dato;
                    var msgResponse = result.Msg;
                    if (datResponse) {
                        option.success(datResponse);
                    } else {
                        PROYECTOMED.MostrarMensaje(msgResponse, 'Error');
                    }

                } else {
                    if (!result.state) {

                        if (result.statusCode === 401) {
                            PROYECTOMED.MostrarMensaje("Acceso no autorizado", 'Error');
                        }

                        if (result.statusCode === 404) {
                            PROYECTOMED.MostrarMensaje("Página no encontrada", 'Error');
                        }

                        if (result.statusCode === 405) {
                            PROYECTOMED.MostrarMensaje("Acceso no autorizado", 'Error');
                        }

                    }
                }
            },
            complete: function () {
                if (option.progress) {
                    Desbloquear();
                }
            }
        });
    },
    //
    AjaxDownload: function (url_download) {
        //Bloquear("Espere un momento...");
        blockUI();
        $.ajax({
            type: "POST",
            async: false,
            url: PROYECTOMED.UrlActionMethod("Access", "KeyToken"),// option.applicatioPath + "../Access/KeyToken",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                result = PROYECTOMED.GetResult(result);
                var datToken = result.Dato;
                var msgToken = result.Msg;
                if (datToken) {

                    if (PROYECTOMED.IsEncrypt()) {
                        datToken = Encrypt(datToken, PROYECTOMED.GetEncryptClv());
                    }
                    var xhr = new XMLHttpRequest();
                    xhr.open('GET', url_download, true);
                    xhr.setRequestHeader("Authorization", "Bearer " + datToken);
                    xhr.responseType = "blob";
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState == 4) {
                            var win = window.open('_blank');
                            var url = URL.createObjectURL(xhr.response);
                            win.location = url;
                            unblockUI();


                            //const a = document.createElement('a');
                            //document.body.appendChild(a);
                            //const url = window.URL.createObjectURL(xhr.response);
                            //a.href = url;
                            //a.target = "_new"
                            //a.download = "Prueba_123";
                            //a.click();
                            //setTimeout(() => {
                            //    window.URL.revokeObjectURL(url);
                            //    document.body.removeChild(a);
                            //}, 0)

                            //unblockUI();
                        }
                    };
                    xhr.send(null);






                } else {
                    xhr.send(null);
                    //Desbloquear();
                    unblockUI();
                }


            },
            complete: function () {
                // Desbloquear();
                //unblockUI();
            }

        });

    },

    //
    AjaxDownloadFileName: function (url_download, file_name) {
        //Bloquear("Espere un momento...");
        blockUI();
        $.ajax({
            type: "POST",
            async: false,
            url: PROYECTOMED.UrlActionMethod("Access", "KeyToken"),// option.applicatioPath + "../Access/KeyToken",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                result = PROYECTOMED.GetResult(result);
                var datToken = result.Dato;
                var msgToken = result.Msg;
                if (datToken) {

                    if (PROYECTOMED.IsEncrypt()) {
                        datToken = Encrypt(datToken, PROYECTOMED.GetEncryptClv());
                    }
                    var xhr = new XMLHttpRequest();
                    xhr.open('GET', url_download, true);
                    xhr.setRequestHeader("Authorization", "Bearer " + datToken);
                    xhr.responseType = "blob";
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState == 4) {
                            const a = document.createElement('a');
                            document.body.appendChild(a);
                            const url = window.URL.createObjectURL(xhr.response);
                            a.href = url;
                            a.target = "_blank"
                            a.download = file_name;
                            a.click();
                            setTimeout(() => {
                                window.URL.revokeObjectURL(url);
                                document.body.removeChild(a);
                            }, 0)

                            unblockUI();
                        }
                    };
                    xhr.send(null);






                } else {
                    xhr.send(null);
                    //Desbloquear();
                    unblockUI();
                }


            },
            complete: function () {
                // Desbloquear();
                //unblockUI();
            }

        });

    },
};

function blockUI() {
    $(function () {
        $.blockUI({ message: '<h4>Espere un momento...</h4>' });
    });
}

function unblockUI() {
    $(function () {
        $.unblockUI();
    });
}

/*------------------------*/
//funciones
function NoPermitirDobleEspacioKeyPress(e) {

    var key = e.keyCode || e.which;
    var tecla = String.fromCharCode(key).toLowerCase();
    var value = e.target.value + "" + tecla;

    var existe = ExisteDobleEspacio(value);
    if (existe) {
        e.preventDefault();
    }
}
function NoPermitirDobleEspacio(campo) {
    if (typeof campo == "string") {
        $(campo).keypress(function (e) {
            //
            var char = String.fromCharCode(e.which);
            var value = this.value + "" + char;
            var existe = ExisteDobleEspacio(value);
            if (existe) {
                return false;
            }
        });
    } else {
        campo.keypress(function (e) {
            var char = String.fromCharCode(e.which);
            var value = this.value + "" + char;
            var existe = ExisteDobleEspacio(value);
            if (existe) {
                //return false;
                e.preventDefault();
            }
        });
    }
}
function ExisteDobleEspacio(value) {
    value = value.replace(/\r?\n/g, ""); //por si tiene enter
    return /\s+\s+/g.test(value);
}

function GetSizeFileMb(size) {
    var resultSize = 0;

    size = size / (1024 * 1024);
    resultSize = size.toFixed(2);

    return parseFloat(resultSize);
}
function GetSizeFileString(size) {
    var resultSize = 0;
    var result = "";

    //Bytes
    if (size <= 1024) {
        result = size + "Bytes";
    } else {

        //kiloBytes
        size = size / 1024;
        resultSize = size.toFixed(2);

        if (size <= 1024) {
            result = resultSize + "Kb";
        } else {

            //MegaBytes
            size = size / 1024;
            resultSize = size.toFixed(2);

            if (size <= 1024) {
                result = resultSize + "Mb";
            } else {
                //GigaBytes
                size = size / 1024;
                resultSize = size.toFixed(2);

                if (size <= 1024) {
                    result = resultSize + "Gb";
                } else {
                    //TeraBytes
                    size = size / 1024;
                    resultSize = size.toFixed(2);
                    result = resultSize + "Tb";

                }
            }
        }
    }

    return result;
}

/***ENCRYPT****/
function Encrypt(textValue, keyValue) {

    if (textValue === null) {
        return null;
    }
    if (textValue === undefined) {
        return undefined;
    }
    if (textValue.replace(/^\s+|\s+$/gm, '') === "") {
        return textValue;
    }
    var key = CryptoJS.enc.Utf8.parse(keyValue);
    var result = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(textValue), key,
        {
            keySize: 128 / 8, iv: key,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    var encrypted = result.toString();

    return encodeURIComponent(encrypted);
}
function Decrypt(encryptedText, pkey) {
    if (encryptedText === null) {
        return null;
    }
    if (encryptedText === undefined) {
        return undefined;
    }

    if (encryptedText.replace(/^\s+|\s+$/gm, '') === "") {
        return encryptedText;
    }
    var key = CryptoJS.enc.Utf8.parse(pkey);

    encryptedText = decodeURIComponent(encryptedText);

    var result = CryptoJS.AES.decrypt(encryptedText, key,
        {
            keySize: 128 / 8, iv: key,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    var decrypted = result.toString(CryptoJS.enc.Utf8);
    return decrypted;
}

function isJSON(item) {
    item = typeof item !== "string"
        ? JSON.stringify(item)
        : item;

    try {
        item = JSON.parse(item);
    } catch (e) {
        return false;
    }

    if (typeof item === "object" && item !== null) {
        return true;
    }

    return false;
}


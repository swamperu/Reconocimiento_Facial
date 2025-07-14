$(document).ready(function () {
    $("#div_captcha").css("display", "none");
    var conteo = parseInt($("#hfConteo").val());
    if (conteo >= 3) {
        $("#div_captcha").css("display", "block");
    }
    var nombre = "";
    var version = "";
    var ua = navigator.userAgent, tem,
        M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        nombre = 'IE ' + (tem[1] || '');
        if (parseFloat((tem[1] || '')) < 10) {
            return;
        }
    }
    if (M[1] === 'Chrome') {
        tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
        if (tem != null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
    nombre = M[0];
    version = M[1];
    if (nombre.toUpperCase() == "MSIE" && parseFloat(version) <= 10) {
        return;
    }
    window.history.pushState(null, "", window.location.href);
    window.onpopstate = function () {
        window.history.pushState(null, "", window.location.href);
    };

    $("[id$=btnLogin]").click(function () {
        FUNCIONES.Login();
        return false;
    });

    $("[id$=Contrasenia]").keydown(function (e) {
        if (e.keyCode == 13) {
            FUNCIONES.Login();
        }
    });

    $("[id$=btnRefresh]").click(function () {
        createCaptcha();
        return false;
    });

    $("[id$=btnVideo]").click(function () {
        var url = 'https://drive.google.com/drive/folders/12KkyHGgG6ATvS1zhro0AJ3QmSH51G7in';
        window.open(url, '_blank');
        return false;
    });

    $("[id$=btnManual]").click(function () {
        var url = 'https://drive.google.com/drive/folders/1PiHHmg5qBHjsbzvH-K-hPQtfGFMSubdh';
        window.open(url, '_blank');
        return false;
    });

    var code;
    function createCaptcha() {
        //clear the contents of captcha div first
        document.getElementById('captchaDiv').innerHTML = "";
        var charsArray =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var lengthOtp = 6;
        var captcha = [];
        for (var i = 0; i < lengthOtp; i++) {
            //below code will not allow Repetition of Characters
            var index = Math.floor(Math.random() * charsArray.length + 1); //get the next character from the array
            if (captcha.indexOf(charsArray[index]) == -1)
                captcha.push(charsArray[index]);
            else i--;
        }
        var canv = document.createElement("canvas");
        canv.id = "captcha";
        canv.width = 120;
        canv.height = 50;
        var ctx = canv.getContext("2d");
        ctx.font = "25px Georgia";
        ctx.strokeText(captcha.join(""), 10, 30);

        ctx.beginPath();
        ctx.moveTo(getRandomInt(10, 50), getRandomInt(10, 50));
        ctx.bezierCurveTo(10, 50, 10, 50, 100, 10);
        ctx.stroke();

        ctx.beginPath();
        ctx.moveTo(getRandomInt(10, 50), getRandomInt(10, 50));
        ctx.bezierCurveTo(20, 80, 200, 10, 200, 10);
        ctx.stroke();

        ctx.beginPath();
        ctx.moveTo(getRandomInt(10, 50), getRandomInt(10, 50));
        ctx.bezierCurveTo(10, 65, 25, 40, 100, 40);
        ctx.stroke();

        //storing captcha so that can validate you can save it somewhere else according to your specific requirements
        code = captcha.join("");
        document.getElementById("captchaDiv").appendChild(canv); // adds the canvas to the body element
    }

    createCaptcha();
    var FUNCIONES = {
        //
        Login: function () {
            $("#lblMsj").html("");
            var datParam = new Object();
            datParam.NombreUsuario = $("[id$=NombreUsuario]").val().toUpperCase();
            datParam.Contrasenia = $("[id$=Contrasenia]").val();
            datParam.CaptchaValido = code; // removeSpaces(document.getElementById('txtCodeCaptcha').innerHTML);
            datParam.Captcha = removeSpaces(document.getElementById('Captcha').value);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: window.location.href.replace("Account/Presentacion","") + "/Account/Login",
                data: JSON.stringify({ 'credenciales': Encrypt(JSON.stringify(datParam)) }),
                success: function (result) {
                    result = FUNCIONES.GetResult(result);
                    var datTabla = result.Dato;
                    var msgTabla = result.Msg;
                    if (datTabla) {
                        window.location.href = window.location.href.replace("Account/Presentacion", "") + "/Home/Index";
                    } else {
                        $("#lblMsj").html(msgTabla);
                        conteo = conteo + 1;
                        if (conteo >= 3) {
                            createCaptcha();
                            $("#div_captcha").css("display", "block");
                        }
                    }
                },
            });
        },
        //
        GetResult: function (result) {
            var token = $("#hfToken").val();
            if (result == undefined)
                return;

            var decrypted = Decrypt(result, token);

            if (isJSON(decrypted)) {
                result = JSON.parse(decrypted);
            } else {
                result = decrypted;
            }
            return result;
        },

    }

    function removeSpaces(str) {
        return str = str.replace(/\s+/g, '');
    }

    /***ENCRYPT****/
    function Encrypt(textValue) {
        var keyValue = $("#hfToken").val();
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

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }



});
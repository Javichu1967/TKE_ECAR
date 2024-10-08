$.validator.unobtrusive.adapters.addSingleVal("forcefilecarnet", "fieldni");

$.validator.addMethod("forcefilecarnet", function (value, element, fieldni) {

    var valueDni = $("#" + fieldni).val();

    if (value == "" && valueDni != "" && valueDni.charAt(0).toUpperCase() == "X") {

        return false;

    }
    return true;
});
$.validator.unobtrusive.adapters.add('requiredconditional', ['fieldconditional', 'requiredisequal'], function (options) {


    var fieldconditionalid = "#" + options.params.fieldconditional;

    if ($(fieldconditionalid).is(':radio')) {
        fieldconditionalid = fieldconditionalid + ":checked";
    }
    options.params.fieldconditional = fieldconditionalid;

    options.rules["requiredconditional"] = options.params;
    options.messages['requiredconditional'] = options.message;

});

$.validator.addMethod("requiredconditional", function (value, element, params) {

    var valueConditional = $(params.fieldconditional).val();

    if (value == "" && valueConditional.toUpperCase() == params.requiredisequal.toUpperCase()) {

        return false;

    }
    return true;
});


$.validator.setDefaults({ ignore: null }); //para validar campos ocultos


$.validator.addMethod("genericcompare", function (value, element, params) {
    var propelename = params.split(",")[0];
    var operName = params.split(",")[1];
    if (params == undefined || params == null || params.length == 0 ||
    value == undefined || value == null || value.length == 0 ||
    propelename == undefined || propelename == null || propelename.length == 0 ||
    operName == undefined || operName == null || operName.length == 0)
        return true;
    var valueOther = $(propelename).val();
    var val1 = (isNaN(value) ? ParseDate(value) : eval(value)); // Date.parse(value)
    var val2 = (isNaN(valueOther) ? ParseDate(valueOther) : eval(valueOther));
    //Si no tiene valor el objeto con el que se debe comparar, no se realiza la comparación
    if (val2 != undefined) {
        if (operName == "GreaterThan")
            return val1 > val2;
        if (operName == "LessThan")
            return val1 < val2;
        if (operName == "GreaterThanOrEqual")
            return val1 >= val2;
        if (operName == "LessThanOrEqual")
            return val1 <= val2;
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.add("genericcompare",
["comparetopropertyname", "operatorname"], function (options) {
    options.rules["genericcompare"] = "#" +
    options.params.comparetopropertyname + "," + options.params.operatorname;
    options.messages["genericcompare"] = options.message;
});
function ParseDate(dateString) {
    // dd/mm/yyyy, or dd/mm/yy
    var dateArr = dateString.split("/");
    if (dateArr.length == 1) {
        return null;    //wrong format
    }
    //parse time after the year - separated by space
    var spacePos = dateArr[2].indexOf(" ");
    if (spacePos > 1) {
        var timeString = dateArr[2].substr(spacePos + 1);
        var timeArr = timeString.split(":");
        dateArr[2] = dateArr[2].substr(0, spacePos);
        if (timeArr.length == 2) {
            //minutes only
            return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]));
        } else {
            //including seconds
            return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]), parseInt(timeArr[2]))
        }
    } else {
        //gotcha at months - January is at 0, not 1 as one would expect
        return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]));
    }
}



//INICIO Validación Tarjeta de combustible *****************************************************************
$.validator.unobtrusive.adapters.addSingleVal("tarjetacombustibleasociada", "fielmatricula");

$.validator.addMethod("tarjetacombustibleasociada", function (value, element, fielmatricula) {
    var valueMatricula = $("#" + fielmatricula).val();
    if (value != "" && valueMatricula != "") {
        var matriculaAsociada = GetMatriculaAsociadaTarjetaCombustible(value);
        if (matriculaAsociada == "") {
            return true;
        }
        else if (matriculaAsociada.toLocaleUpperCase() != valueMatricula.toLocaleUpperCase()) {
            //var required_string = GetGlobalResourceObject("Resource", "required");
            return false;
        }
    }
    return true;
});

function GetMatriculaAsociadaTarjetaCombustible(tarjeta) {
    var matriculaAsociada = "";
    var uRl = uRl_TarjetaCombustible.replace("idtarjetaseleccionada", tarjeta);

    $.ajax({
        "url": uRl,
        "dataType": 'json',
        "async": false,
        "success": function (data) {
            matriculaAsociada = data;
        }
    });

    return matriculaAsociada;
}
//FIN Validación Tarjeta de combustible *****************************************************************

//INICIO Validación Matrícula sustituida *****************************************************************
//  No pueden ser la misma matrícula.
$.validator.unobtrusive.adapters.addSingleVal("matriculasustituida", "fielmatricula");

$.validator.addMethod("matriculasustituida", function (value, element, fielmatricula) {
    var valueMatricula = $("#" + fielmatricula).val();
    if (value != "" && valueMatricula != "") {
        if (value.toLocaleUpperCase() == valueMatricula.toLocaleUpperCase()) {
            return false;
        }
    }
    return true;
});
//FIN Validación Matrícula sustituida *****************************************************************

//INICIO Validación Matrícula existente *****************************************************************
$.validator.unobtrusive.adapters.addSingleVal("matriculaexistente", "fielaccion");

$.validator.addMethod("matriculaexistente", function (value, element, fielaccion) {
    var valorReturn = true;
    var valueAccion = $("#" + fielaccion).val();
    if (valueAccion == AccionAlta) {
        var uRl = uRl_MatriculaExistente.replace("matriculaNueva", value);

        $.ajax({
            "url": uRl,
            "dataType": 'json',
            "async": false,
            "success": function (data) {
                if (data == "Existente") {
                    valorReturn = false;
                }
            }
        });
    }
    return valorReturn;
});
//FIN Validación Matrícula existente *****************************************************************

//INICIO Validación Matrícula sustitución vacía ******************************************************
$.validator.unobtrusive.adapters.addSingleVal("matriculasustitucionvacia", "fielessustitucion");

$.validator.addMethod("matriculasustitucionvacia", function (value, element, fielessustitucion) {
    var valueEsSustitucion = $("#" + fielessustitucion).val();
    if (value == "" && valueEsSustitucion == "True") {
        return false;
    }
    return true;
});
//FIN Validación Matrícula sustitución vacía ***********************************************************

//INICIO Validación Fecha Vto. ITV vacía *****************************************************************
$.validator.unobtrusive.adapters.addSingleVal("fechaultimaitvnoobligatoriaenprimeraitv", "fiellinea");

$.validator.addMethod("fechaultimaitvnoobligatoriaenprimeraitv", function (value, element, fiellinea) {
    var valorReturn = true;
    var valueLinea = $("#" + fiellinea).val();
    var valueFechaUltimaITV = value;
    if (valueFechaUltimaITV == null || valueFechaUltimaITV == "") {
        tableMtoITV.rows().every(function (rowindx, tableLoop, rowLoop) {
            var data = this.data();
            if (data.FechaUltimaITV == null && data.Linea != valueLinea) {
                valorReturn = false;
            }
        });
    }
    return valorReturn;
});
//FIN Validación Fecha Vto. ITV vacía *****************************************************************

//INICIO Validación Fechas ITV correctas. *****************************************************************
//  No debe haber ninguna entre el intérvalo de ultima y vto. *********************************************
$.validator.unobtrusive.adapters.add("fechasitvcorrectas", ["fielfechavto", "fiellinea"], function (options) {
    options.rules["fechasitvcorrectas"] = [options.params.fielfechavto, options.params.fiellinea];
    options.messages["fechasitvcorrectas"] = options.message;
});

$.validator.addMethod("fechasitvcorrectas", function (value, element, params) {
    var valorReturn = true;
    var valueLinea = $("#" + params[1]).val(); //$("#" + fiellinea).val();
    var valueFechaVto = $("#" + params[0]).val(); //fielfechavto;
    var valueFechaUltimaITV = value;
    if (valueFechaVto != null && valueFechaVto != "" && valueFechaUltimaITV != null && valueFechaUltimaITV != "") {
        var dateFrom = convertToDate(valueFechaUltimaITV);
        var dateTo = convertToDate(valueFechaVto);
        tableMtoITV.rows().every(function (rowindx, tableLoop, rowLoop) {
            var data = this.data();
            if (data.FechaVtoITV != null && data.Linea != valueLinea) {
                var dateFromCompare = convertToDate(formatJSONDate(data.FechaUltimaITV));
                var dateToCompare = convertToDate(formatJSONDate(data.FechaVtoITV));
                if (dateFromCompare > dateFrom && dateToCompare < dateTo) {
                    valorReturn = false;
                }
            }
        });
    }
    return valorReturn;
});
//FIN Validación Fechas ITV correctas *****************************************************************


//INICIO Validación Fechas ITV correctas. *****************************************************************
//  Debe haber rellena alguna de las fechas ***************************************************************
$.validator.unobtrusive.adapters.addSingleVal("fechasitvrellenas", "fielfechavto");

$.validator.addMethod("fechasitvrellenas", function (value, element, fielfechavto) {
    var valorReturn = true;
    var valueFechaVTO = $("#" + fielfechavto).val();
    var valueFechaUltimaITV = value;

    if (valueFechaUltimaITV == "" && valueFechaVTO == "") {
        valorReturn = false;
    }

    return valorReturn;
});
//FIN Validación Fechas ITV correctas *****************************************************************


//INICIO Validación Fecha última ITV vacía en primera ITV *********************************************
$.validator.unobtrusive.adapters.addBool("identificadorimportacionunico");

$.validator.addMethod("identificadorimportacionunico", function (value, element) {
    var valorReturn = true;
    var valueIdentificador = value;

    if (value != undefined && value != null) {
        var uRl = uRl_IdentificadorUnico.replace("identificadorimportacion", value);
        $.ajax({
            "url": uRl,
            "dataType": 'json',
            "async": false,
            "success": function (data) {
                if (!data) {
                    valorReturn = false;
                }
            }
        });
    }

    return valorReturn;
});
//FIN Validación Fecha última ITV vacía en primera ITV **************************************************

//INICIO Validación Documento Vehículo *********************************************
$.validator.unobtrusive.adapters.addBool("documentovehiculorequerido");

$.validator.addMethod("documentovehiculorequerido", function (value, element) {
    var valorReturn = true;
    var valueDocumento = value;

    if (value == undefined || value == null || value == '') {
        valorReturn = false;
    }

    return valorReturn;
});
//FIN Validación Fecha última ITV vacía en primera ITV **************************************************

//INICIO Validación que el campo tenga valor *********************************************
$.validator.unobtrusive.adapters.addBool("noisempty");

$.validator.addMethod("noisempty", function (value, element) {
    var valorReturn = true;
    var valueIdentificador = value;

    if (value != undefined && value != null && value != "") {
        valorReturn = false;
    }

    return valorReturn;
});
//FIN Validación que el campo tenga valor **************************************************

//INICIO Validación PIN tarjeta combustible *****************************************************************
$.validator.unobtrusive.adapters.addSingleVal("tarjetacombustiblepinobligatorio", "fielempresa");

$.validator.addMethod("tarjetacombustiblepinobligatorio", function (value, element, fielempresa) {
    var valorReturn = true;
    var valueEmpresa = $("#" + fielempresa).val();
    var valuePin = value;

    if (valueEmpresa == "8150" && (valuePin == null || valuePin == "")) {
        valorReturn = false;
    }

    return valorReturn;
});
//FIN Validación PIN tarjeta combustible *****************************************************************

//INICIO Validación ITV ya pasada *********************************************
$.validator.unobtrusive.adapters.addBool("valoritvyapasada");

$.validator.addMethod("valoritvyapasada", function (value, element) {
    var valorReturn = true;
    var valueITV_Pasada = value;
    if (valueITV_Pasada == 'false') {
        tableMtoITV.rows().every(function (rowindx, tableLoop, rowLoop) {
            var data = this.data();
            if (data.ITV_Pasada == false) {
                    valorReturn = false;
            }
        });
    }

    return valorReturn;
});
//FIN Validación ITV ya pasada **************************************************

//INICIO Validación Extensión documento Excel. *********************************************
$.validator.unobtrusive.adapters.addBool("excelextensioncorrecta");

$.validator.addMethod("excelextensioncorrecta", function (value, element) {
    var valorReturn = true;
    var valueFile= value;

    if (valueFile != "") {
        var ext = valueFile.split('.').pop();
        if (ext.toUpperCase() == "XLS") {
            valorReturn = false;
        }
    }

    return valorReturn;
});
//FIN Validación Extensión documento Excel. **************************************************




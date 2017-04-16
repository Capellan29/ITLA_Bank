var body;
var tabla = document.createElement("table");
var tblHead = document.createElement("thead");
var tblBody = document.createElement("tbody");
var monto, tasa, plazo, interes, amortizado, deuda, cuota;

function calcular()
{
    monto = parseFloat(document.getElementById("monto").value);
    plazo = parseInt(document.getElementById("plazo").value);
    tasa = parseFloat(document.getElementById("tasa").value);
    tasa = tasa /12/100;
    var divisor = 1 + tasa;
    divisor = Math.pow(divisor,plazo * -1);
    divisor = 1 - divisor;
    cuota = tasa / divisor;
    cuota = cuota * monto;
    deuda = monto;
}

function genera_tabla() {
    eliminar_tabla();
    calcular();
    body = document.getElementById("body");
    tabla = document.createElement("table");
    tblHead = document.createElement("thead");
    var hilera = document.createElement("tr");
    titulos = new Array("Periodo", "Cuota", "Interes", "Amortizado", "Deuda");
    titulos.forEach(function (item, titulos) {        
        var celda = document.createElement("th");
        var textoCelda = document.createTextNode(item);
        celda.appendChild(textoCelda);
        hilera.appendChild(celda);
    });

    tblHead.appendChild(hilera);
    tabla.appendChild(tblHead);

    tblBody = document.createElement("tbody");
    for (var i = 1; i <= plazo; i++) {

        interes = tasa * deuda;
        amortizado = cuota - interes;
        deuda -= amortizado;
        var amortizadoImprimmir = amortizado.toFixed(2);
        valores = new Array(i, cuota.toFixed(2), interes.toFixed(2), amortizadoImprimmir, deuda.toFixed(2));
        hilera = document.createElement("tr");
        
        for (var j = 0; j < 5; j++){
            celda = document.createElement("td");
            textoCelda = document.createTextNode(valores[j]);
            celda.appendChild(textoCelda);
            hilera.appendChild(celda);
        }
        tblBody.appendChild(hilera);
    }

    tabla.appendChild(tblBody);
    body.appendChild(tabla);
    tabla.setAttribute("class", "table table-hover")
}

function eliminar_tabla(){
    tabla.remove();
}

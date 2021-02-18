"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var express = require("express");
var cors = require("cors");
var bodyParser = require("body-parser");
var gramaticaErrores = require("./AnalizadorJava/analizadorErrores");
var gramaticaJSTREE = require("./AnalizadorJava/analizadorJSTREE");
var gramaticaArbol = require("./AnalizadorJava/analizadorArbol");
var app = express();
app.use(bodyParser.json());
app.use(cors());
app.use(bodyParser.urlencoded({ extended: true }));
app.post('/Calcular/', function (req, res) {
    var entrada = req.body.text;
    var resultado = parser(entrada);
    res.send(resultado);
});

app.post('/Comprobar/', function(req, res){
    var texto1 = req.body.text1;
    var texto2 = req.body.text2;

    var resultado1 = parser2(texto1);
    var resultado2 = parser2(texto2);

    var analizar = comprobarCopias(resultado1, resultado2);

    res.send(analizar);
});

function Clase(nombre, funciones, metodos, variables) {
    this.nombre = nombre
    this.funciones = funciones
    this.metodos = metodos
    this.variables = variables
}

function Funcion(nombre, tipo, parametros, variables) {
    this.nombre = nombre
    this.tipo = tipo
    this.parametros = parametros
    this.variables = variables
}

function Metodo(nombre, parametros, variables){
    this.nombre = nombre
    this.parametros = parametros
    this.variables = variables
}

function Parametro(tipo, nombre) {
    this.tipo = tipo
    this.nombre = nombre
}

function Variable(tipo, nombre){
    this.tipo = tipo
    this.nombre = nombre
}

var clasesArbol = []
var clasesArbol1 = []
var clasesArbol2 = []
var clase, metodo, funcion, parametro, variable

function comprobarCopias(arbol1, arbol2){
    var raizArbol1 = arbol1
    var raizArbol2 = arbol2

    analizarArbol(raizArbol1)
    clasesArbol1 = clasesArbol
    clasesArbol = []
    analizarArbol(raizArbol2)
    clasesArbol2 = clasesArbol

    var copias = [copiasClase(), copiasFunciones(), copiasVariables()]

    return transformarTexto(copias)
}

function transformarTexto(copias){
    var textoClase, textoFunciones, textoVariables

    for(var a = 0; a < copias[0].length; a++){

        var nombre = copias[0][a].nombre
        var funciones = copias[0][a].funciones.length
        var metodos = copias[0][a].metodos.length

        if(a === 0){
            textoClase = "Clase: " + nombre + " Funciones: " + funciones + " Metodos: " + metodos 
        } else {
            textoClase += "\nClase: " + nombre + " Funciones: " + funciones + " Metodos: " + metodos
        }
    }

    for(var a = 0; a < copias[1].length; a++){
        for(var b = 0; b < copias[1][a].length; b++){
            var nombreClase = copias[1][a][b].clase
            var nombreFuncion = copias[1][a][b].funcion.nombre
            var parametros = "{"

            for(var c = 0; c < copias[1][a][b].funcion.parametros.length; c++){
                if(copias[1][a][b].funcion.parametros[c] !== undefined){
                    if(c === 0){
                        parametros += "\n\t\t\t" + copias[1][a][b].funcion.parametros[c].tipo + " " + copias[1][a][b].funcion.parametros[c].nombre + "\n"
                    }else{
                        parametros += "\t\t\t" + copias[1][a][b].funcion.parametros[c].tipo + " " + copias[1][a][b].funcion.parametros[c].nombre + "\n"
                    }
                }
            }
            parametros += "\t\t}"

            if(a === 0){
                textoFunciones = "Clase: " + nombreClase + "\n\tFuncion: " + nombreFuncion + "\n\t\tParametros: " + parametros 
            } else {
                textoFunciones += "\nClase: " + nombreClase + "\n\tFuncion: " + nombreFuncion + "\n\t\tParametros: " + parametros 
            }
        }
    }

    for(var a = 0; a < copias[2].length; a++){
        var clase = copias[2][a][0].clase
        var funmet = copias[2][a][0].funmet 
        var tipo = copias[2][a][0].variable.tipo
        var nombre = copias[2][a][0].variable.nombre

        if(a === 0){
            textoVariables = "Clase: " + clase + "\n\tMetodo-Funcion: " + funmet + "\n\t\tTipo: " + tipo + "\n\t\tNombre: " + nombre
        } else {
            textoVariables += "\nClase: " + clase + "\n\tMetodo-Funcion: " + funmet + "\n\t\tTipo: " + tipo + "\n\t\tNombre: " + nombre
        }
    }

    return [textoClase, textoFunciones, textoVariables]
}

function copiasClase(){
    var copias = []

    for(var i = 0; i < clasesArbol1.length; i++){
        var nombreClase1 = clasesArbol1[i].nombre
        var funcionesClase1 = clasesArbol1[i].funciones
        var metodosClase1 = clasesArbol1[i].metodos


        for(var j = 0; j < clasesArbol2.length; j++){
            var nombreClase2 = clasesArbol2[j].nombre
            var funcionesClase2 = clasesArbol2[j].funciones
            var metodosClase2 = clasesArbol2[j].metodos

            if(nombreClase1 === nombreClase2){
                //Los nombres de las clases coinciden
                if(funcionesClase1.length === funcionesClase2.length && metodosClase1.length === metodosClase2.length){

                    //Verificar funciones
                    var cantidadFunciones = funcionesClase1.length
                    for(var k = 0; k < funcionesClase1.length; k++){
                        for(var l = 0; l < funcionesClase2.length; l++){

                            //Hay match de nombres
                            if(funcionesClase1[k].nombre === funcionesClase2[l].nombre){
                                cantidadFunciones--
                            }
                        }
                    }

                    if(cantidadFunciones === 0){
                        //Todas las funciones son iguales

                        //Verificar funciones
                        var cantidadMetodos = metodosClase1.length
                        for(var k = 0; k < metodosClase1.length; k++){
                            for(var l = 0; l < metodosClase2.length; l++){

                                //Hay match de nombres
                                if(metodosClase1[k].nombre === metodosClase2[l].nombre){
                                    cantidadMetodos--
                                }
                            }
                        }

                        if(cantidadMetodos === 0){
                            //Todos los metodos son iguales. Hay copia

                            copias.push(clasesArbol1[i])
                        }
                    }
                }
            }
        }
    }

    return copias
}

function copiasFunciones(){
    var copias = []

    for(var i = 0; i < clasesArbol1.length; i++){
        var nombreClase1 = clasesArbol1[i].nombre
        var funcionesClase1 = clasesArbol1[i].funciones
        var metodosClase1 = clasesArbol1[i].metodos


        for(var j = 0; j < clasesArbol2.length; j++){
            var nombreClase2 = clasesArbol2[j].nombre
            var funcionesClase2 = clasesArbol2[j].funciones
            var metodosClase2 = clasesArbol2[j].metodos

            if(nombreClase1 === nombreClase2){
                //Los nombres de las clases coinciden
                if(funcionesClase1.length === funcionesClase2.length && metodosClase1.length === metodosClase2.length){

                    //Verificar funciones
                    for(var k = 0; k < funcionesClase1.length; k++){
                        for(var l = 0; l < funcionesClase2.length; l++){

                            //Hay match de nombres
                            if(funcionesClase1[k].nombre === funcionesClase2[l].nombre){

                                var parametrosClase1 = funcionesClase1[k].parametros
                                var parametrosClase2 = funcionesClase2[l].parametros

                                var hayCopia = true
                                for(var m = 0; m < parametrosClase1.length; m++){
                                    if(parametrosClase1[m].tipo === parametrosClase2[m].tipo){
                                        hayCopia = true
                                    }else{
                                        hayCopia = false
                                    }

                                }

                                if(hayCopia){
                                    copias.push([{clase: nombreClase1, funcion: funcionesClase1[k]}])
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return copias
}

function copiasVariables(){
    var copias = []

    for(var i = 0; i < clasesArbol1.length; i++){
        var nombreClase1 = clasesArbol1[i].nombre
        var funcionesClase1 = clasesArbol1[i].funciones
        var metodosClase1 = clasesArbol1[i].metodos
        var variablesClase1 = clasesArbol1[i].variables

        for(var j = 0; j < clasesArbol2.length; j++){
            var nombreClase2 = clasesArbol2[j].nombre
            var funcionesClase2 = clasesArbol2[j].funciones
            var metodosClase2 = clasesArbol2[j].metodos
            var variablesClase2 = clasesArbol2[j].variables

            if(nombreClase1 === nombreClase2){
                //Los nombres de las clases coinciden
                if(funcionesClase1.length === funcionesClase2.length && metodosClase1.length === metodosClase2.length){

                    //Verificar funciones
                    for(var k = 0; k < funcionesClase1.length; k++){
                        for(var l = 0; l < funcionesClase2.length; l++){

                            //Hay match de nombres
                            if(funcionesClase1[k].nombre === funcionesClase2[l].nombre){

                                var varClase1 = funcionesClase1[k].variables
                                var varClase2 = funcionesClase2[l].variables

                                for(var m = 0; m < varClase1.length; m++){
                                    for(var n = 0; n < varClase2.length; n++){
                                        if(varClase1[m].tipo === varClase2[n].tipo && varClase1[m].nombre === varClase2[n].nombre){
                                            copias.push([{clase: nombreClase1, funme: funcionesClase1[k].nombre, variable: varClase1[m]}])
                                        }
                                    }
                                }

                            }
                        }
                    }

                    //Verificar funciones
                    for(var k = 0; k < metodosClase1.length; k++){
                        for(var l = 0; l < metodosClase2.length; l++){

                            //Hay match de nombres
                            if(metodosClase1[k].nombre === metodosClase2[l].nombre){

                                var varClase1 = metodosClase1[k].variables
                                var varClase2 = metodosClase2[l].variables

                                for(var m = 0; m < varClase1.length; m++){
                                    for(var n = 0; n < varClase2.length; n++){
                                        if(varClase1[m].tipo === varClase2[n].tipo && varClase1[m].nombre === varClase2[n].nombre){
                                            copias.push([{clase: nombreClase1, funmet: metodosClase1[k].nombre, variable: varClase1[m]}])
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            for(var m = 0; m < variablesClase1.length; m++){
                for(var n = 0; n < variablesClase2.length; n++){
                    if(variablesClase1[m].tipo === variablesClase2[n].tipo && variablesClase1[m].nombre === variablesClase2[n].nombre){
                        copias.push([{clase: nombreClase1, funmet: "Global", variable: variablesClase1[m]}])
                    }
                }
            }
        }
    }

    return copias
}

function analizarArbol(raiz){
    if(raiz.hijos != undefined){
        if(raiz.nombre == "Clase"){
            clase = new Clase(raiz.valor, [], [], [])
            analizarClase(raiz)
            var contador = 0
            for(var i = 0; i < clasesArbol.length; i++){
                if(clasesArbol[i].nombre === clase.nombre){
                    contador++
                }
            }
            if(contador === 0){
                clasesArbol.push(clase)
            }

            var inst = raiz.hijos[0]

            for(var i = 0; i < inst.hijos.length; i++){
                if(inst.hijos[i].nombre == "Declaracion"){
                    for(var j = 0; j < inst.hijos[i].hijos[1].hijos.length; j++){
                        variable = new Variable(inst.hijos[i].hijos[0], inst.hijos[i].hijos[1].hijos[j])
                        clase.variables.push(variable)
                    }
                }
            }
        }
        for(var i = 0; i < raiz.hijos.length; i++){
            analizarArbol(raiz.hijos[i])
        }
    }
}

function analizarParametrosMetodo(raiz){
    if(raiz.hijos != undefined){
        if(raiz.nombre == "Parametro"){
            for(var i = 0; i < raiz.hijos.length; i++){
                parametro = new Parametro(raiz.valor, raiz.hijos[0].valor)
                metodo.parametros.push(parametro)
            }
        }
        for(var i = 0; i < raiz.hijos.length; i++){
            analizarParametrosMetodo(raiz.hijos[i])
        }
    }
}

function analizarMetodo(raiz){
    if(raiz.hijos != undefined){
        if(raiz.nombre == "Declaracion"){
            for(var i = 0; i < raiz.hijos[1].hijos.length; i++){
                variable = new Variable(raiz.hijos[0], raiz.hijos[1].hijos[i])
                metodo.variables.push(variable)
            }
        }
        for(var i = 0; i < raiz.hijos.length; i++){
            analizarMetodo(raiz.hijos[i])
        }
    }
}

function analizarParametrosFuncion(raiz){
    if(raiz.hijos != undefined){
        if(raiz.nombre == "Parametro"){
            for(var i = 0; i < raiz.hijos.length; i++){
                parametro = new Parametro(raiz.valor, raiz.hijos[0].valor)
                funcion.parametros.push(parametro)
            }
        }
        for(var i = 0; i < raiz.hijos.length; i++){
            analizarParametrosFuncion(raiz.hijos[i])
        }
    }
}

function analizarFuncion(raiz){
    if(raiz.hijos != undefined){
        if(raiz.nombre == "Declaracion"){
            for(var i = 0; i < raiz.hijos[1].hijos.length; i++){
                variable = new Variable(raiz.hijos[0], raiz.hijos[1].hijos[i])
                funcion.variables.push(variable)
            }
        }
        for(var i = 0; i < raiz.hijos.length; i++){
            analizarFuncion(raiz.hijos[i])
        }
    }
}

function analizarClase(raiz){
    if(raiz.hijos != undefined){
        if(raiz.nombre == "Metodo"){
            metodo = new Metodo(raiz.valor, [], [])
            if(raiz.hijos.length === 2){
                analizarParametrosMetodo(raiz.hijos[0])
                analizarMetodo(raiz.hijos[1])
            }else{
                analizarMetodo(raiz.hijos[0])
            }
            clase.metodos.push(metodo)

        }
        if(raiz.nombre == "Funcion"){
            funcion = new Funcion(raiz.valor, raiz.hijos[0], [], [], [])
            analizarParametrosFuncion(raiz.hijos[1])
            analizarFuncion(raiz.hijos[2])
            clase.funciones.push(funcion)

        }
        for(var i = 0; i < raiz.hijos.length; i++){
            analizarClase(raiz.hijos[i])
        }
    }
}

function parser2(texto) {
    try {
        return gramaticaArbol.parse(texto);
    }
    catch (e) {
        return "Error en compilacion de Entrada: " + e.toString();
    }
}


/*---------------------------------------------------------------*/
var server = app.listen(8080, function () {
    console.log('Servidor escuchando en puerto 8080...');
});
/*---------------------------------------------------------------*/
function parser(texto) {
    try {
        var resultadoAnalisis1 = gramaticaErrores.parse(texto);
        if(resultadoAnalisis1.toString() == ""){
            console.log(gramaticaJSTREE.parse(texto))
            return gramaticaJSTREE.parse(texto);
        }
        return resultadoAnalisis1
        //return gramaticaArbol.parse(texto);
    }
    catch (e) {
        return "Error en compilacion de Entrada: " + e.toString();
    }
}
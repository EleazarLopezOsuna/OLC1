function Error(tipo, lexema, linea, columna, mensaje){
    this.tipo = tipo
    this.lexema = lexema
    this.linea = linea
    this.columna = columna
    this.mensaje = mensaje
}

function Conn(){

    var texto = document.getElementById("operacion").value;

    var url='http://localhost:8080/Calcular/';

    $.post(url,{text:texto},function(data,status){
        if(status.toString()=="success"){
            if(data.toString().startsWith('E-')){
                alert("Texto Erroneo")
                var listaErrores = []
                var errores = data.split("\n");

                for(var i = 0; i < errores.length; i++){
                    var prop = errores[i].split("-")
                    var tipo = prop[1]
                    var lexema = prop[2]
                    var linea = prop[3]
                    var columna = prop[4]

                    if(tipo === "S"){
                        listaErrores.push(new Error("Sintactico", lexema, linea, columna, "No se esperaba el token: "))
                    }
                }

                reportarErrores(listaErrores)
                document.getElementById('traduccion').disabled = true
                create_node([])
            }else{
                alert("Texto Correcto")
                var a = JSON.stringify(data)

                console.log(a)

                create_node(JSON.parse(a))

                var child = document.getElementById('cabeceras').lastElementChild;  
                while (child) { 
                    document.getElementById('cabeceras').removeChild(child); 
                    child = document.getElementById('cabeceras').lastElementChild; 
                }
            
                child = document.getElementById('cuerpo').lastElementChild;  
                while (child) { 
                    document.getElementById('cuerpo').removeChild(child); 
                    child = document.getElementById('cuerpo').lastElementChild; 
                }
                
                document.getElementById('traduccion').disabled = false
            }
        }else{
            alert("Error estado de conexion:"+status);
        }
    });
}

function Comparar(){

    var texto1 = document.getElementById("operacion").value;
    var texto2 = document.getElementById("traduccion").value;

    var url='http://localhost:8080/Comprobar/';

    $.post(url,{text1:texto1, text2:texto2},function(data,status){
        if(status.toString()=="success"){
            //REPORTAR LO ENCONTRADO
            guardarReportes(data)
        }else{
            alert("Error estado de conexion:"+status);
        }
    });
}
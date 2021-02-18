SET JAVA_HOME = "C:\Program Files\Java\jdk-11.0.2\bin";
SET PATH =% JAVA_HOME%;%PATH%;
SET CLASSPATH = %JAVA_HOME%;
cd C:\Users\USER\Documents\NetBeansProjects\Practica1_Compiladores_SegundoSemestre_2019\src\Analizadores
java -jar C:\Users\USER\Desktop\Analizadores\java-cup-11b.jar -parser Analisis_Sintactico_Reportes -symbols Simbolos_Reportes Sintactico_Reportes.cup
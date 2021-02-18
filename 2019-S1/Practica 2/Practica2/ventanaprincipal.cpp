#include "ventanaprincipal.h"
#include "ui_ventanaprincipal.h"
#include "stdio.h"
#include "qfiledialog.h"
#include <QFile>
#include <QTextStream>

//Variables Globales
QString path = "";

VentanaPrincipal::VentanaPrincipal(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::VentanaPrincipal)
{
    ui->setupUi(this);
}

VentanaPrincipal::~VentanaPrincipal()
{
    delete ui;
}

void VentanaPrincipal::on_actionNuevo_triggered()
{
    QString fileName = QFileDialog::getSaveFileName(this,tr("Crear Documento"), "",tr("Documentos FI (*.fi)"));
    if(fileName != NULL){
        ui -> textEdit -> setEnabled(true);
        QFileInfo fi(fileName);
        path = fi.filePath();
        this->setWindowTitle(path);
    }
}

void VentanaPrincipal::on_actionGuardar_2_triggered()
{
    Escribir();
}

void VentanaPrincipal::on_actionGuardar_Como_triggered()
{
    QString fileName = QFileDialog::getSaveFileName(this,tr("Crear Documento"), "",tr("Documentos FI (*.fi)"));
    if(fileName != NULL){
        ui -> textEdit -> setEnabled(true);
        QFileInfo fi(fileName);
        path = fi.filePath();
        this->setWindowTitle(path);
        Escribir();
    }
}


void VentanaPrincipal::Escribir(){
    QFile file( path );
    if(file.open(QIODevice::WriteOnly | QIODevice::Text))
       {
           QTextStream stream(&file);
           QString texto = ui -> textEdit -> toPlainText();
           QStringList lines = texto.split("\n");
           for(int i = 0; i < lines.size(); i++){
               stream << lines.at(i)+"\n";
           }
           file.close();
       }
}

void VentanaPrincipal::on_actionAbrir_triggered(){
    QString fileName = QFileDialog::getOpenFileName(this,tr("Abrir Documento"), "",tr("Documentos FI (*.fi)"));
    if(fileName != NULL){
        QFileInfo fi(fileName);
        path = fi.filePath();
        this->setWindowTitle(path);
        QFile file(path);
        if(file.open(QIODevice::ReadOnly|QIODevice::Text)){
            ui -> textEdit -> setPlainText("");
            QTextStream stream(&file);
            QString line;
            do {
                line = stream.readLine();
                QString texto = ui->textEdit->toPlainText();
                ui -> textEdit -> setPlainText(texto+line+"\n");
            } while(!line.isNull());
            file.close();
            ui -> textEdit -> setEnabled(true);
         }
    }
}

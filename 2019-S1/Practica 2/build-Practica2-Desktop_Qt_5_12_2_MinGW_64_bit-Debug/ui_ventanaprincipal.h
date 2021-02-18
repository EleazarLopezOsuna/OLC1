/********************************************************************************
** Form generated from reading UI file 'ventanaprincipal.ui'
**
** Created by: Qt User Interface Compiler version 5.12.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_VENTANAPRINCIPAL_H
#define UI_VENTANAPRINCIPAL_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QGridLayout>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QListWidget>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenu>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QTabWidget>
#include <QtWidgets/QTableWidget>
#include <QtWidgets/QTextEdit>
#include <QtWidgets/QToolBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_VentanaPrincipal
{
public:
    QAction *actionNuevo;
    QAction *actionGuardar_2;
    QAction *actionGuardar_Como;
    QAction *actionAbrir;
    QAction *actionCompilar;
    QAction *actionTokes;
    QAction *actionLexico;
    QAction *actionSintactico;
    QAction *actionGenerar_AST;
    QWidget *centralWidget;
    QWidget *gridLayoutWidget;
    QGridLayout *gridLayout;
    QTextEdit *textEdit;
    QTabWidget *tabWidget;
    QWidget *tab;
    QListWidget *listWidget;
    QWidget *tab_2;
    QTableWidget *tableWidget;
    QMenuBar *menuBar;
    QMenu *menuVentana_Principal;
    QMenu *menuGuardar;
    QMenu *menuHerramientas;
    QMenu *menuReportes;
    QMenu *menuErrores;
    QToolBar *mainToolBar;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *VentanaPrincipal)
    {
        if (VentanaPrincipal->objectName().isEmpty())
            VentanaPrincipal->setObjectName(QString::fromUtf8("VentanaPrincipal"));
        VentanaPrincipal->setEnabled(true);
        VentanaPrincipal->resize(752, 488);
        QSizePolicy sizePolicy(QSizePolicy::Preferred, QSizePolicy::Preferred);
        sizePolicy.setHorizontalStretch(0);
        sizePolicy.setVerticalStretch(0);
        sizePolicy.setHeightForWidth(VentanaPrincipal->sizePolicy().hasHeightForWidth());
        VentanaPrincipal->setSizePolicy(sizePolicy);
        VentanaPrincipal->setMinimumSize(QSize(752, 488));
        VentanaPrincipal->setMaximumSize(QSize(752, 488));
        actionNuevo = new QAction(VentanaPrincipal);
        actionNuevo->setObjectName(QString::fromUtf8("actionNuevo"));
        actionGuardar_2 = new QAction(VentanaPrincipal);
        actionGuardar_2->setObjectName(QString::fromUtf8("actionGuardar_2"));
        actionGuardar_Como = new QAction(VentanaPrincipal);
        actionGuardar_Como->setObjectName(QString::fromUtf8("actionGuardar_Como"));
        actionAbrir = new QAction(VentanaPrincipal);
        actionAbrir->setObjectName(QString::fromUtf8("actionAbrir"));
        actionCompilar = new QAction(VentanaPrincipal);
        actionCompilar->setObjectName(QString::fromUtf8("actionCompilar"));
        actionTokes = new QAction(VentanaPrincipal);
        actionTokes->setObjectName(QString::fromUtf8("actionTokes"));
        actionLexico = new QAction(VentanaPrincipal);
        actionLexico->setObjectName(QString::fromUtf8("actionLexico"));
        actionSintactico = new QAction(VentanaPrincipal);
        actionSintactico->setObjectName(QString::fromUtf8("actionSintactico"));
        actionGenerar_AST = new QAction(VentanaPrincipal);
        actionGenerar_AST->setObjectName(QString::fromUtf8("actionGenerar_AST"));
        centralWidget = new QWidget(VentanaPrincipal);
        centralWidget->setObjectName(QString::fromUtf8("centralWidget"));
        gridLayoutWidget = new QWidget(centralWidget);
        gridLayoutWidget->setObjectName(QString::fromUtf8("gridLayoutWidget"));
        gridLayoutWidget->setGeometry(QRect(0, 0, 751, 231));
        gridLayout = new QGridLayout(gridLayoutWidget);
        gridLayout->setSpacing(6);
        gridLayout->setContentsMargins(11, 11, 11, 11);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        gridLayout->setContentsMargins(0, 0, 0, 0);
        textEdit = new QTextEdit(gridLayoutWidget);
        textEdit->setObjectName(QString::fromUtf8("textEdit"));
        textEdit->setEnabled(false);

        gridLayout->addWidget(textEdit, 0, 0, 1, 1);

        tabWidget = new QTabWidget(centralWidget);
        tabWidget->setObjectName(QString::fromUtf8("tabWidget"));
        tabWidget->setGeometry(QRect(0, 230, 751, 211));
        QSizePolicy sizePolicy1(QSizePolicy::Fixed, QSizePolicy::Fixed);
        sizePolicy1.setHorizontalStretch(0);
        sizePolicy1.setVerticalStretch(0);
        sizePolicy1.setHeightForWidth(tabWidget->sizePolicy().hasHeightForWidth());
        tabWidget->setSizePolicy(sizePolicy1);
        tab = new QWidget();
        tab->setObjectName(QString::fromUtf8("tab"));
        listWidget = new QListWidget(tab);
        listWidget->setObjectName(QString::fromUtf8("listWidget"));
        listWidget->setGeometry(QRect(0, 0, 751, 181));
        tabWidget->addTab(tab, QString());
        tab_2 = new QWidget();
        tab_2->setObjectName(QString::fromUtf8("tab_2"));
        tableWidget = new QTableWidget(tab_2);
        if (tableWidget->columnCount() < 6)
            tableWidget->setColumnCount(6);
        QTableWidgetItem *__qtablewidgetitem = new QTableWidgetItem();
        tableWidget->setHorizontalHeaderItem(0, __qtablewidgetitem);
        QTableWidgetItem *__qtablewidgetitem1 = new QTableWidgetItem();
        tableWidget->setHorizontalHeaderItem(1, __qtablewidgetitem1);
        QTableWidgetItem *__qtablewidgetitem2 = new QTableWidgetItem();
        tableWidget->setHorizontalHeaderItem(2, __qtablewidgetitem2);
        QTableWidgetItem *__qtablewidgetitem3 = new QTableWidgetItem();
        tableWidget->setHorizontalHeaderItem(3, __qtablewidgetitem3);
        QTableWidgetItem *__qtablewidgetitem4 = new QTableWidgetItem();
        tableWidget->setHorizontalHeaderItem(4, __qtablewidgetitem4);
        QTableWidgetItem *__qtablewidgetitem5 = new QTableWidgetItem();
        tableWidget->setHorizontalHeaderItem(5, __qtablewidgetitem5);
        tableWidget->setObjectName(QString::fromUtf8("tableWidget"));
        tableWidget->setGeometry(QRect(0, 0, 751, 161));
        tableWidget->setLineWidth(1);
        tableWidget->setDragDropOverwriteMode(true);
        tableWidget->horizontalHeader()->setDefaultSectionSize(124);
        tabWidget->addTab(tab_2, QString());
        VentanaPrincipal->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(VentanaPrincipal);
        menuBar->setObjectName(QString::fromUtf8("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 752, 21));
        menuVentana_Principal = new QMenu(menuBar);
        menuVentana_Principal->setObjectName(QString::fromUtf8("menuVentana_Principal"));
        menuGuardar = new QMenu(menuVentana_Principal);
        menuGuardar->setObjectName(QString::fromUtf8("menuGuardar"));
        menuHerramientas = new QMenu(menuBar);
        menuHerramientas->setObjectName(QString::fromUtf8("menuHerramientas"));
        menuReportes = new QMenu(menuBar);
        menuReportes->setObjectName(QString::fromUtf8("menuReportes"));
        menuErrores = new QMenu(menuReportes);
        menuErrores->setObjectName(QString::fromUtf8("menuErrores"));
        VentanaPrincipal->setMenuBar(menuBar);
        mainToolBar = new QToolBar(VentanaPrincipal);
        mainToolBar->setObjectName(QString::fromUtf8("mainToolBar"));
        VentanaPrincipal->addToolBar(Qt::TopToolBarArea, mainToolBar);
        statusBar = new QStatusBar(VentanaPrincipal);
        statusBar->setObjectName(QString::fromUtf8("statusBar"));
        VentanaPrincipal->setStatusBar(statusBar);

        menuBar->addAction(menuVentana_Principal->menuAction());
        menuBar->addAction(menuHerramientas->menuAction());
        menuBar->addAction(menuReportes->menuAction());
        menuVentana_Principal->addAction(actionNuevo);
        menuVentana_Principal->addAction(menuGuardar->menuAction());
        menuVentana_Principal->addAction(actionAbrir);
        menuGuardar->addAction(actionGuardar_2);
        menuGuardar->addAction(actionGuardar_Como);
        menuHerramientas->addAction(actionCompilar);
        menuHerramientas->addAction(actionGenerar_AST);
        menuReportes->addAction(menuErrores->menuAction());
        menuReportes->addAction(actionTokes);
        menuErrores->addAction(actionLexico);
        menuErrores->addAction(actionSintactico);

        retranslateUi(VentanaPrincipal);

        tabWidget->setCurrentIndex(0);


        QMetaObject::connectSlotsByName(VentanaPrincipal);
    } // setupUi

    void retranslateUi(QMainWindow *VentanaPrincipal)
    {
        VentanaPrincipal->setWindowTitle(QApplication::translate("VentanaPrincipal", "Practica 2", nullptr));
        actionNuevo->setText(QApplication::translate("VentanaPrincipal", "Nuevo", nullptr));
        actionGuardar_2->setText(QApplication::translate("VentanaPrincipal", "Guardar", nullptr));
        actionGuardar_Como->setText(QApplication::translate("VentanaPrincipal", "Guardar Como", nullptr));
        actionAbrir->setText(QApplication::translate("VentanaPrincipal", "Abrir", nullptr));
        actionCompilar->setText(QApplication::translate("VentanaPrincipal", "Compilar", nullptr));
        actionTokes->setText(QApplication::translate("VentanaPrincipal", "Tokens", nullptr));
        actionLexico->setText(QApplication::translate("VentanaPrincipal", "Lexico", nullptr));
        actionSintactico->setText(QApplication::translate("VentanaPrincipal", "Sintactico", nullptr));
        actionGenerar_AST->setText(QApplication::translate("VentanaPrincipal", "Generar AST", nullptr));
#ifndef QT_NO_TOOLTIP
        tabWidget->setToolTip(QString());
#endif // QT_NO_TOOLTIP
        tabWidget->setTabText(tabWidget->indexOf(tab), QApplication::translate("VentanaPrincipal", "Consola", nullptr));
        QTableWidgetItem *___qtablewidgetitem = tableWidget->horizontalHeaderItem(0);
        ___qtablewidgetitem->setText(QApplication::translate("VentanaPrincipal", "Linea", nullptr));
        QTableWidgetItem *___qtablewidgetitem1 = tableWidget->horizontalHeaderItem(1);
        ___qtablewidgetitem1->setText(QApplication::translate("VentanaPrincipal", "Columna", nullptr));
        QTableWidgetItem *___qtablewidgetitem2 = tableWidget->horizontalHeaderItem(2);
        ___qtablewidgetitem2->setText(QApplication::translate("VentanaPrincipal", "Id", nullptr));
        QTableWidgetItem *___qtablewidgetitem3 = tableWidget->horizontalHeaderItem(3);
        ___qtablewidgetitem3->setText(QApplication::translate("VentanaPrincipal", "Tipo", nullptr));
        QTableWidgetItem *___qtablewidgetitem4 = tableWidget->horizontalHeaderItem(4);
        ___qtablewidgetitem4->setText(QApplication::translate("VentanaPrincipal", "Valor Inicial", nullptr));
        QTableWidgetItem *___qtablewidgetitem5 = tableWidget->horizontalHeaderItem(5);
        ___qtablewidgetitem5->setText(QApplication::translate("VentanaPrincipal", "Valor Final", nullptr));
        tabWidget->setTabText(tabWidget->indexOf(tab_2), QApplication::translate("VentanaPrincipal", "Variables", nullptr));
        menuVentana_Principal->setTitle(QApplication::translate("VentanaPrincipal", "Funcionalidades", nullptr));
        menuGuardar->setTitle(QApplication::translate("VentanaPrincipal", "Guardar", nullptr));
        menuHerramientas->setTitle(QApplication::translate("VentanaPrincipal", "Herramientas", nullptr));
        menuReportes->setTitle(QApplication::translate("VentanaPrincipal", "Reportes", nullptr));
        menuErrores->setTitle(QApplication::translate("VentanaPrincipal", "Errores", nullptr));
    } // retranslateUi

};

namespace Ui {
    class VentanaPrincipal: public Ui_VentanaPrincipal {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_VENTANAPRINCIPAL_H

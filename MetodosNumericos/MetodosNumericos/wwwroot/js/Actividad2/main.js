import * as FN from './functions.js';
import * as MT from './methods.js';

let anim = document.getElementById("anim");
let btngraf = document.getElementById("graf");
let btnfn = document.getElementsByClassName('btnfn');
let btnmtd = document.getElementsByClassName('btnmtd');
let inpa = document.getElementById('ainput');
let inpb = document.getElementById('binput');
let inptol = document.getElementById('tol');
let intp0 = document.getElementById("p0");
let intp1 = document.getElementById('p1');
let intmop = document.getElementById('miter');
let lblraiz = document.getElementById('lblraiz');
let lblerr = document.getElementById('lblerr');
let slider = document.getElementById("slider");
let sliderValue = document.getElementById("sliderV");
let menu = document.getElementById("pol");
let func;
let method;
let tablaG;
let lineheight;
let pol;
let options = {
    a: ['0', '1', '2'],
    b: ['0', '1', '2'],
    c: ['0', '1', '2'],
    d: ['0', '1'],
    e: ['0'],

}



//Funciones para obtener el arreglo de coordenadas y graficar
const funciones = {
    a: (a, b, pas) => FN.a(a, b, pas),  // x³ - 2x² - 5
    b: (a, b, pas) => FN.b(a, b, pas),  // x³ + 3x² - 1
    c: (a, b, pas) => FN.c(a, b, pas),  // x - cos(x)
    d: (a, b, pas) => FN.d(a, b, pas),
    e: (a, b, pas) => FN.e(a, b, pas)  // e^x + 2^(-x) + 2cos(x) - 6
};
//Polinomios
const poli = [
    { a: "(10-4x^2)^(1/3)",  
        b: "(5+2x^2)^(1/3)",
        c: "(1-3x^2)^(1/3)",
        d: "cos(x)",
        e: "e^x + 2^(-x) + 2cos(x) - 6 + x" },
    {a: "((10-x^3)/4)^(1/2)",  
        b: "((x^3-5)/2)^(1/2)",
        c: "((1-x^3)/3)^(1/2)",
        d: "2x - cos(x)",
    },
    {
        a:"x³+4x²+x-10",
        b: "x³-2x²+x-5",
        c: "x³+3x²+x-1",
    },
]

//valores por default al seleccionar una funcion
const defaultFN = {
    // a   b   tol     iteraciones maximas
    a: [1, 3, 0.00001, 20],  // x³ - 2x² - 5
    b: [2.5, 3, 0.00001, 20],  // x³ + 3x² - 1
    c: [-1, 0 , 0.00001, 20],  // x - cos(x)
    d: [0, 1.5707, 0.0001, 20],
    e: [1.5, 2, 0.00001, 20]  // e^x + 2^(-x) + 2cos(x) - 6
};

//configuracion de la grafica
//rline, gline, tline son lineas de apoyo para las animaciones

let myChart;
export function initializeChart2() {
    const config = {
        type: 'line',
        data: {},
        options: {
            animation: false,
            scales: {
                x: {
                    min: -1,
                    max: 1,
                    type: 'linear',
                    position: 'bottom',
                    title: {
                        display: true,
                        text: 'x'
                    },

                },
                y: {
                    min: -1,
                    max: 1,
                    title: {
                        display: true,
                        text: 'y'
                    },
                }
            },
            plugins: {
                annotation: {
                    annotations: {
                        line1: {
                            type: 'line',
                            xMin: 0, // Posición de la línea vertical
                            xMax: 0,
                            borderColor: 'black', // Color de la línea
                            borderWidth: 1, // Grosor de la línea
                            label: {
                                content: 'x = 2',
                                enabled: true,
                                position: 'top'
                            }
                        },
                        line2: {
                            type: 'line',
                            yMin: 0, // Posición de la línea vertical
                            yMax: 0,
                            borderColor: 'black', // Color de la línea
                            borderWidth: 1, // Grosor de la línea

                            label: {
                                content: 'x = 2',
                                enabled: true,
                                position: 'top'
                            }
                        },
                        rline: { //linea de apoyo
                            type: 'line',
                            xMin: -100000, // Posición de la línea vertical
                            xMax: -100000,
                            yMin: NaN,
                            yMax: NaN,
                            borderColor: 'rgb(162, 0, 255)', // Color de la línea
                            borderWidth: 1, // Grosor de la línea
                            borderDash: [5, 5], // Línea punteada (opcional)
                            label: {
                                content: 'x = 2',
                                enabled: true,
                                position: 'top'
                            }
                        },
                        gline: { //linea de apoyo
                            type: 'line',
                            xMin: -100000, // Posición de la línea vertical
                            xMax: -100000,
                            yMin: NaN,
                            yMax: NaN,
                            borderColor: 'rgb(162, 0, 255)', // Color de la línea
                            borderWidth: 1, // Grosor de la línea
                            borderDash: [5, 5], // Línea punteada (opcional)
                            label: {
                                content: 'x = 2',
                                enabled: true,
                                position: 'top'
                            }
                        },
                        tline: { //linea de apoyo
                            type: 'line',
                            xMin: -100000, // Posición de la línea vertical
                            xMax: -100000,
                            yMin: NaN,
                            yMax: NaN,
                            borderColor: 'rgb(162, 0, 255)', // Color de la línea
                            borderWidth: 1, // Grosor de la línea
                            borderDash: [5, 5], // Línea punteada (opcional)
                            label: {
                                content: 'x = 2',
                                enabled: true,
                                position: 'top'
                            }
                        },
                    }
                },
                zoom: {
                    pan: { enabled: true, mode: 'xy', modifierKey: 'ctrl' },
                    zoom: { wheel: { enabled: true }, pinch: { enabled: true }, mode: 'xy' }
                }
            }
        },
    };

    const ctx = document.getElementById('myChart').getContext('2d');

    myChart = new Chart(ctx, config);

    //Variables para el manejo del programa
    anim = document.getElementById("anim");
     btngraf = document.getElementById("graf");
     btnfn = document.getElementsByClassName('btnfn');
     btnmtd = document.getElementsByClassName('btnmtd');
     inpa = document.getElementById('ainput');
     inpb = document.getElementById('binput');
     inptol = document.getElementById('tol');
     intp0 = document.getElementById("p0");
     intp1 = document.getElementById('p1');
     intmop = document.getElementById('miter');
    lblraiz = document.getElementById('lblraiz');
    lblerr = document.getElementById('lblerr');
     slider = document.getElementById("slider");
     sliderValue = document.getElementById("sliderV");
     menu = document.getElementById("pol");
     func;
     method;
     tablaG;
     lineheight;
     pol;
     options = {
        a: ['0', '1', '2'],
        b: ['0', '1', '2'],
        c: ['0', '1', '2'],
        d: ['0', '1'],
        e: ['0'],

    }



    menu.style.display = "none";

    Array.from(btnfn).forEach(boton => {
        boton.addEventListener('click', function () {
            console.log('ID del botón presionado: ' + this.id);
            if (func) {
                const butant = document.getElementById(func);
                butant.style.backgroundColor = ' rgb(141, 209, 38)';

            }
            func = this.id
            this.style.backgroundColor = "#0a0a0a";





            inpa.value = defaultFN[this.id][0];
            inpb.value = defaultFN[this.id][1];
            inptol.value = defaultFN[this.id][2];
            intmop.value = defaultFN[this.id][3];

            if (method === 'btnPF') {
                menu.style.display = "block";
                menu.innerHTML = "";
                let i = 0;
                options[func].forEach(option => {
                    let item = document.createElement("a");
                    item.classList.add("pol-item"); // Agrega una clase
                    item.id = String(i); // Agrega un id único
                    item.textContent = poli[option][func];
                    item.onclick = () => {
                        if (pol) {
                            const ant = document.getElementById(pol);
                            ant.style.backgroundColor = " rgb(141, 209, 38)";
                        }
                        const it = document.getElementById(item.id);
                        it.style.backgroundColor = "#0a0a0a";
                        pol = item.id
                    };
                    menu.appendChild(item);
                    i++;
                });
            }

        });
    });


    Array.from(btnmtd).forEach(boton => {
        boton.addEventListener('click', function () {
            console.log('ID del botón presionado: ' + this.id);
            if (method) {
                const butant = document.getElementById(method);
                butant.style.backgroundColor = " rgb(141, 209, 38)";

            }
            method = this.id
            this.style.backgroundColor = "#0a0a0a";

            if (this.id === 'btnPF' || this.id === 'btnNR') {
                intp1.style.display = "none";
                if (this.id === 'btnPF') {
                    menu.style.display = "block";
                    menu.innerHTML = "";
                    let i = 0;
                    options[func].forEach(option => {
                        let item = document.createElement("a");
                        item.classList.add("pol-item"); // Agrega una clase
                        item.id = String(i); // Agrega un id único
                        item.textContent = poli[option][func];
                        item.onclick = () => {
                            if (pol) {
                                const ant = document.getElementById(pol);
                                ant.style.backgroundColor = " rgb(141, 209, 38)";
                            }
                            const it = document.getElementById(item.id);
                            it.style.backgroundColor = "#0a0a0a";
                            pol = item.id
                        };
                        menu.appendChild(item);
                        i++;
                    });
                }
            } else {
                intp1.style.display = "";
                menu.style.display = "none";
            }

        });
    });

    const setUpFN = {
        btnPF: () => setUpPF(inpa.value, inpb.value),
        btnNR: () => setUpNR(),
        btnBS: () => setUpBS(),
        btnSC: () => setUpSC(),
        btnRF: () => setUpRF(),
        btnRFM: () => setUpRFM()
    }

    const actualizarFn = {
        btnPF: x => actualizarPF(x),
        btnNR: x => actualizarNR(x),
        btnBS: x => actualizarBS(x),
        btnSC: x => actualizarSC(x),
        btnRF: x => actualizarRF(x),
        btnRFM: x => actualizarRFM(x)
    }

    btngraf.addEventListener('click', function () {
        //LIMPIAR GRAFICA
        resetF();
        graficar(func);
        const setUp = setUpFN[method]
        const act = actualizarFn[method]
        setUp()
        act(1)

    });

    anim.addEventListener('click', function () {
        const delay = 1000;   // Slower animation (increase for slower effect)
        resetF();
        graficar(func, document.getElementById('ainput').value, document.getElementById('binput').value);
        const setUp = setUpFN[method]
        const act = actualizarFn[method]
        setUp()
        let x = parseInt(slider.value);
        if (x == tablaG.length - 1) {
            x = 1
        }
        function animateChart() {
            if (x > tablaG.length - 1) return; // Stop animation
            act(x)
            x += 1;
            setTimeout(animateChart, delay); // Add delay
        }
        animateChart();

    });


    slider.addEventListener("input", function () {
        sliderValue.textContent = slider.value; // Change the value of <a>
        const act = actualizarFn[method]
        act(parseInt(slider.value))
    });
}

window.initializeChart2 = initializeChart2;


//Reset grafics
function resetF(){
    myChart.data.datasets = []
    myChart.options.plugins.annotation.annotations['rline'].xMin = -100000;
    myChart.options.plugins.annotation.annotations['rline'].xMax = -100000;
    myChart.options.plugins.annotation.annotations['rline'].yMin = NaN;
    myChart.options.plugins.annotation.annotations['rline'].yMax = NaN;
    myChart.options.plugins.annotation.annotations['gline'].xMin = -100000;
    myChart.options.plugins.annotation.annotations['gline'].xMax = -100000;
    myChart.options.plugins.annotation.annotations['gline'].yMin = NaN;
    myChart.options.plugins.annotation.annotations['gline'].yMax = NaN;
    myChart.options.plugins.annotation.annotations['tline'].xMin = -100000;
    myChart.options.plugins.annotation.annotations['tline'].xMax = -100000;
    myChart.options.plugins.annotation.annotations['tline'].yMin = NaN;
    myChart.options.plugins.annotation.annotations['tline'].yMax = NaN;
}

function graphHorizontalLine(line, x, x1, y){
    myChart.options.plugins.annotation.annotations[line].yMin =y;
    myChart.options.plugins.annotation.annotations[line].yMax =y;
    if(x<x1){
        myChart.options.plugins.annotation.annotations[line].xMin =x;
        myChart.options.plugins.annotation.annotations[line].xMax =x1; 
    }else{
        myChart.options.plugins.annotation.annotations[line].xMin =x1;
        myChart.options.plugins.annotation.annotations[line].xMax =x; 
    }
}

function graphVerticalLine(line, y, y1, x){
    myChart.options.plugins.annotation.annotations[line].xMin =x;
    myChart.options.plugins.annotation.annotations[line].xMax =x;
    if(y<y1){
        myChart.options.plugins.annotation.annotations[line].yMin =y;
        myChart.options.plugins.annotation.annotations[line].yMax =y1; 
    }else{
        myChart.options.plugins.annotation.annotations[line].yMin =y1;
        myChart.options.plugins.annotation.annotations[line].yMax =y; 
    }
      
}

function actlblRaiz(er, raiz, tot, x){
    console.log(er, parseFloat(inptol.value), x, tot)
    if (Math.abs(er)>parseFloat(inptol.value) && x==tot){
        lblraiz.textContent = 'Raiz: sin resultado';
        console.log("ENTRO");
    }else{
        lblraiz.textContent = 'Raiz: ' + raiz;
        console.log('raiz')
    }
}





function graficar(gr, inpaValue, inpbValue){
    const funcion = funciones[gr]
    const { data, minY, maxY } = funcion(parseFloat(inpa.value), parseFloat(inpb.value), 100);
    console.log("hola", data)
    myChart.data.datasets.push({
        label: document.getElementById(gr).textContent,
        data: data, // Y values
        borderColor: 'blue',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });
    myChart.options.scales.y.min =minY //miny;
    myChart.options.scales.y.max =maxY//maxy;
    myChart.options.scales.x.min = parseFloat(inpa.value);
    myChart.options.scales.x.max = parseFloat(inpb.value);
    console.log("aquiiii", myChart.data)
    myChart.update()
}

function generarTabla(arreglo) {
    const tabla = document.getElementById("tabla");
    tabla.innerHTML = ""; // Limpiar tabla antes de generar nueva

    // Crear filas y celdas dinámicamente
    arreglo.forEach((fila, i) => {
        let tr = document.createElement("tr");

        fila.forEach(celda => {
            let elemento = i === 0 ? document.createElement("th") : document.createElement("td");
            elemento.textContent = celda;
            tr.appendChild(elemento);
        });

        tabla.appendChild(tr);
    });
}

//Funciones de setup para graficar

function setUpNR(){
    const {raiz, tabla } = MT.newton_r(func, parseFloat(intp0.value), parseFloat(inptol.value), parseInt(intmop.value))
        tabla.unshift(['i', 'p', 'pi+1', 'f(pi+1)', 'f(p)', 'Ea'])
        tablaG =tabla;

        console.log(tabla);
        lblraiz.textContent = 'Raiz: ' + tabla[1][1];
        lblerr.textContent = 'Error absoluto:: ' + tabla[1][5];
        generarTabla(tabla)
        //Agregar el punto que ira animando atravez del tiempo
        myChart.data.datasets.push({
            label: 'p' + parseFloat(tabla[1][1]),
            data: [{x:parseFloat(tabla[1][1]), y : 0}], // Y values
            borderColor: 'red',
            borderWidth: 2,
            fill: false,
            pointRadius: 2
        });
        //Graficar xn
        myChart.data.datasets.push({
            label: 'pi+1',
            data: [], // Y values
            borderColor: 'rgb(0, 255, 255)',
            borderWidth: 2,
            fill: false,
            pointRadius: 2
        });

        //Agregar la linea tangente que ira mostrandose a traves del tiempo
        myChart.data.datasets.push({
            label: 'tangente',
            data: [], // Y values
            borderColor: 'rgba(30, 255, 0, 0.73)',
            borderWidth: 2,
            fill: false,
            pointRadius: 0
        });

       
        slider.max = tabla.length-1;

        myChart.update();
}

function setUpPF(inpaValue, inpbValue){
    console.log(pol)
    const {raiz, tabla } = MT.fixed_point(func, parseFloat(intp0.value), parseInt(pol), parseFloat(inptol.value), parseInt(intmop.value))
    tabla.unshift(['i', 'p', 'p1', 'g(p)', 'f(p)', 'Ea']);
    tablaG=tabla;
   

    generarTabla(tabla)
    slider.max = tabla.length-1;
    //Agregar el punto que ira animando atravez del tiempo
    myChart.data.datasets.push({
        label: 'x' + parseFloat(tabla[1][1]),
        data: [{x:parseFloat(tabla[1][1]), y : 0}], // Y values
        borderColor: 'red',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });
    //Graficar xn
    myChart.data.datasets.push({
        label: 'xn',
        data: [], // Y values
        borderColor: 'rgb(0, 255, 255)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });
    
    //Graficar xn
    myChart.data.datasets.push({
        label: 'g(x)',
        data: [], // Y values
        borderColor: 'rgb(234, 0, 255)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });
    //Grafiacar la funcion identidad
    const d = FN.identidad(parseFloat(inpaValue), parseFloat(inpbValue), 20)
    myChart.data.datasets.push({
        label: 'identidad',
        data: d, // Y values
        borderColor: 'green',
        borderWidth: 2,
        borderDash: [5, 5],
        fill: false,
        pointRadius: 0
    });

    //Graficar la funcion despejada x=g(x)
    const g = FN.pf(parseFloat(inpaValue), parseFloat(inpbValue), pol, func, 30)
    myChart.data.datasets.push({
        label: 'g(x)',
        data:g,  // Y values
        borderColor: 'red',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });
    myChart.update();
}

function setUpBS(){
    const {raiz, tabla } = MT.bisection(func, parseFloat(intp0.value), parseFloat(intp1.value), parseFloat(inptol.value), parseInt(intmop.value))
    tabla.unshift(['i', 'a', 'b', 'c','f(a)', 'f(b)', 'f(c)', 'Error absoluto intervalo']);
    tablaG=tabla;

    generarTabla(tabla)
    slider.max = tabla.length-1;

    //Tomar los valores maximos en la grafica para sacar un porciento para las lineas rectas
    let diferencia =  myChart.options.scales.y.max - myChart.options.scales.y.min;
    //obtenemos los valores para el 2.5%
    lineheight = diferencia*0.025

    //Agregar el punto que ira animando atravez del tiempo
    myChart.data.datasets.push({
        label: 'c = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'red',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });
    //Agregar linea vertical a
    myChart.data.datasets.push({
        label: 'a = ' + parseFloat(tabla[1][1]),
        data: [], // Y values
        borderColor: 'rgb(60, 255, 0)',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });
    //Agregar linea vertical b
    myChart.data.datasets.push({
        label: 'b = ' + parseFloat(tabla[1][2]),
        data: [], // Y values
        borderColor: 'rgb(153, 0, 255)',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });

    myChart.update();
}

function setUpSC() {
    const { raiz, tabla } = MT.secante(func, parseFloat(intp0.value), parseFloat(intp1.value), parseFloat(inptol.value), parseInt(intmop.value));
    tabla.unshift(['i', 'p', 'pi+1', 'f(pi+1)', 'f(p)', 'Ea']);
    tablaG = tabla;

    console.log(tabla);
    lblraiz.textContent = 'Raiz: ' + tabla[1][1];
    lblerr.textContent = 'Error absoluto: ' + tabla[1][5];
    generarTabla(tabla);

    // Agregar el punto f(p)
    myChart.data.datasets.push({
        label: 'f(p) = ' + parseFloat(tabla[1][4]),
        data: [{ x: parseFloat(tabla[1][1]), y: parseFloat(tabla[1][4]) }], 
        borderColor: 'red',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

    // Agregar el punto f(pi+1)
    myChart.data.datasets.push({
        label: 'f(pi+1) = ' + parseFloat(tabla[1][3]),
        data: [{ x: parseFloat(tabla[1][2]), y: parseFloat(tabla[1][3]) }],
        borderColor: 'rgb(0, 255, 255)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

    // Agregar la recta secante que conecta f(p) y f(pi+1)
    myChart.data.datasets.push({
        label: 'Secante f(p)-f(pi+1)',
        data: [],
        borderColor: 'rgba(30, 255, 0, 0.73)',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });

    slider.max = tabla.length - 1;
    myChart.update();
}



function setUpRF(){
    const {raiz, tabla } = MT.false_position(func, parseFloat(intp0.value), parseFloat(intp1.value), parseFloat(inptol.value), parseInt(intmop.value))
    tabla.unshift(['i', 'a', 'b', 'c','f(a)', 'f(b)', 'f(x)', 'Error absoluto raices']);
    tablaG=tabla;

    generarTabla(tabla)
    slider.max = tabla.length-1;

     //Agregar el punto que ira animando atravez del tiempo
     myChart.data.datasets.push({
        label: 'c = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'red',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

     //Agregar el punto f(a)
    myChart.data.datasets.push({
        label: 'f(a) = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'rgb(43, 255, 0)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

    //Agregar el punto f(b)
    myChart.data.datasets.push({
        label: 'f(b) = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'rgb(255, 0, 98)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

     //Recta de a a b
     myChart.data.datasets.push({
        label: 'Recta ab',
        data: [], // Y values
        borderColor: 'rgb(217, 255, 0)',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });
    
    myChart.update();
}

function setUpRFM() {
    const { raiz, tabla } = MT.regula_falsi_modificada(func, parseFloat(intp0.value), parseFloat(intp1.value), parseFloat(inptol.value), parseInt(intmop.value));
    tabla.unshift(['i', 'a', 'b', 'c','f(a)', 'f(b)', 'f(x)', 'Error absoluto raices']);
    tablaG=tabla;

    generarTabla(tabla)
    slider.max = tabla.length-1;

     //Agregar el punto que ira animando atravez del tiempo
     myChart.data.datasets.push({
        label: 'c = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'red',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

     //Agregar el punto f(a)
    myChart.data.datasets.push({
        label: 'f(a) = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'rgb(43, 255, 0)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

    //Agregar el punto f(b)
    myChart.data.datasets.push({
        label: 'f(b) = ' + parseFloat(tabla[1][3]),
        data: [], // Y values
        borderColor: 'rgb(255, 0, 98)',
        borderWidth: 2,
        fill: false,
        pointRadius: 2
    });

     //Recta de a a b
     myChart.data.datasets.push({
        label: 'Recta ab',
        data: [], // Y values
        borderColor: 'rgb(217, 255, 0)',
        borderWidth: 2,
        fill: false,
        pointRadius: 0
    });
    
    myChart.update();
}


//Funciones para actualizar la animacion
function actualizarNR(x){
    slider.value = parseInt(tablaG[x][0])
    sliderValue.textContent = slider.value;
    //Actualizar labels
    actlblRaiz(tablaG[x][5], tablaG[x][1], tablaG.length-1, x)
    lblerr.textContent = 'Error absoluto: ' + tablaG[x][5];
    //Actualizar la posicion del punto
    myChart.data.datasets[1].label = 'p = ' + parseFloat(tablaG[x][1]).toFixed(5);
    myChart.data.datasets[1].data =  [{x:parseFloat(tablaG[x][1]), y : 0}];
    myChart.data.datasets[2].label = 'pi+1 = ' + parseFloat(tablaG[x][2]).toFixed(5);
    myChart.data.datasets[2].data =  [{x:parseFloat(tablaG[x][2]), y : 0}];
    myChart.data.datasets[3].data=[
        {x:parseFloat(tablaG[x][1]), y : parseFloat(tablaG[x][4])},
        {x:parseFloat(tablaG[x][2]), y : 0}
    ]

    //lINEA DE X A la recta tangente  vertical rline
    graphVerticalLine('rline', 0 ,parseFloat(tablaG[x][4]),parseFloat(tablaG[x][1]))

    myChart.update();

}

function actualizarPF(x){
    slider.value = parseInt(tablaG[x][0])
    sliderValue.textContent = slider.value;
    //Actualizar labels
            
    actlblRaiz(tablaG[x][5], tablaG[x][1], tablaG.length-1, x)
    lblerr.textContent = 'Error absoluto: ' + tablaG[x][5];

    //Actualizar la posicion del punto
    myChart.data.datasets[1].label = 'x = ' + parseFloat(tablaG[x][1]).toFixed(5);
    myChart.data.datasets[1].data =  [{x:parseFloat(tablaG[x][1]), y : 0}];
    myChart.data.datasets[2].label = 'xn = ' + parseFloat(tablaG[x][2]).toFixed(5);
    myChart.data.datasets[2].data =  [{x:parseFloat(tablaG[x][2]), y : 0}];
    myChart.data.datasets[3].label= 'g(x) =' + parseFloat(tablaG[x][3]).toFixed(5)
    myChart.data.datasets[3].data=[{x:parseFloat(tablaG[x][1]), y : parseFloat(tablaG[x][3])},]

    //Linea de p a g(x) vertical rline
    graphVerticalLine('rline',0, parseFloat(tablaG[x][3]), parseFloat(tablaG[x][1]) );

    //Linea de g(x) a identidad horizontal gline
    graphHorizontalLine('gline', parseFloat(tablaG[x][1]), parseFloat(tablaG[x][2]), parseFloat(tablaG[x][2]));


    //Linea de identidad a p1 vertical tline
    graphVerticalLine('tline',0, parseFloat(tablaG[x][3]), parseFloat(tablaG[x][2]) );


    myChart.update();
}

function actualizarBS(x){
    slider.value = parseInt(tablaG[x][0])
    sliderValue.textContent = slider.value;
    //Actualizar labels
    actlblRaiz(tablaG[x][5], tablaG[x][3], tablaG.length-1, x)
    lblerr.textContent = 'Error absoluto intervalo: ' + tablaG[x][7];
    //Actualizar el punto c
    myChart.data.datasets[1].label = 'c = ' + parseFloat( tablaG[x][3]).toFixed(5);
    myChart.data.datasets[1].data =  [{x:parseFloat(tablaG[x][3]), y : 0}];
    //Actualizar linea a
    myChart.data.datasets[2].label = 'a = ' + parseFloat( tablaG[x][1]).toFixed(5);
    myChart.data.datasets[2].data =  [
        {x:parseFloat(tablaG[x][1]), y : lineheight},
        {x:parseFloat(tablaG[x][1]), y : -lineheight},

    ];

    //actualizar linea b
    myChart.data.datasets[3].label = 'b = ' + parseFloat( tablaG[x][2]).toFixed(5);
    myChart.data.datasets[3].data =  [
        {x:parseFloat(tablaG[x][2]), y : lineheight},
        {x:parseFloat(tablaG[x][2]), y : -lineheight},

    ];
    myChart.update();
}
function calcularSecanteExtendidaYInterseccion(x0, y0, x1, y1, extension = 2) {
    // Cálculo de pendiente
    const pendiente = (y1 - y0) / (x1 - x0);
    
    // Extender la recta secante en ambas direcciones
    const xInicio = x0 - extension * (x1 - x0);
    const yInicio = y0 - extension * (y1 - y0);
    const xFin = x1 + extension * (x1 - x0);
    const yFin = y1 + extension * (y1 - y0);

    // Calcular intersección con el eje X (f(x) = 0)
    const xInterseccion = x0 - y0 / pendiente; // Despejando de la ecuación de la recta

    return {
        secanteExtendida: [{ x: xInicio, y: yInicio }, { x: xFin, y: yFin }],
        interseccionX: { x: xInterseccion, y: 0 }
    };
}

function actualizarSC(x) {
    slider.value = parseInt(tablaG[x][0]);
    sliderValue.textContent = slider.value;

    // Actualizar labels
    actlblRaiz(tablaG[x][5], tablaG[x][1], tablaG.length - 1, x);
    lblerr.textContent = 'Error absoluto: ' + tablaG[x][5];

    // Obtener valores de la tabla
    const x0 = parseFloat(tablaG[x][1]);
    const y0 = parseFloat(tablaG[x][4]);
    const x1 = parseFloat(tablaG[x][2]);
    const y1 = parseFloat(tablaG[x][3]);

    // Calcular la secante extendida y la intersección con el eje X
    const { secanteExtendida, interseccionX } = calcularSecanteExtendidaYInterseccion(x0, y0, x1, y1);

    // Actualizar el punto p en la gráfica
    myChart.data.datasets[1].label = 'p = ' + x0.toFixed(5);
    myChart.data.datasets[1].data = [{ x: x0, y: y0 }];

    // Actualizar el punto p+1 en la gráfica
    myChart.data.datasets[2].label = 'p+1 = ' + x1.toFixed(5);
    myChart.data.datasets[2].data = [{ x: x1, y: y1 }];

    // Actualizar la línea de la secante extendida
    myChart.data.datasets[3].label = 'Secante';
    myChart.data.datasets[3].data = secanteExtendida;

    // Agregar la intersección con el eje X
    myChart.data.datasets[4] = {
        label: 'Intersección con X',
        data: [interseccionX],
        borderColor: 'blue',
        borderWidth: 2,
        fill: false,
        pointRadius: 4
    };

    myChart.update();
}

function actualizarRF(x){
    slider.value = parseInt(tablaG[x][0])
    sliderValue.textContent = slider.value;
    //Actualizar labels
    actlblRaiz(tablaG[x][7], tablaG[x][3], tablaG.length-1, x)
    lblerr.textContent = 'Error absoluto raices: ' + tablaG[x][7];
    //Actualizar el punto c
    myChart.data.datasets[1].label = 'c = ' + parseFloat( tablaG[x][3]).toFixed(5);
    myChart.data.datasets[1].data = [{x: parseFloat(tablaG[x][3]), y: 0}];

    //Actualizar el punto f(a)
    myChart.data.datasets[2].label = '(a): (' + parseFloat( tablaG[x][1]).toFixed(5) + ' , '+ parseFloat( tablaG[x][4]).toFixed(5) + ')';
    myChart.data.datasets[2].data = [{x: parseFloat(tablaG[x][1]), y: parseFloat(tablaG[x][4])}];


    //Actualizar el punto f(b)
    myChart.data.datasets[3].label = '(b): (' + parseFloat( tablaG[x][2]).toFixed(5) + ' , '+ parseFloat( tablaG[x][5]).toFixed(5) + ')';
    myChart.data.datasets[3].data = [{x:parseFloat(tablaG[x][2]), y : parseFloat(tablaG[x][5])}];


    myChart.data.datasets[4].data.push(
        {x:parseFloat(tablaG[x][1]), y : parseFloat(tablaG[x][4])},
        {x:parseFloat(tablaG[x][2]), y : parseFloat(tablaG[x][5])}
    );

    //linea vertical de a a b pasando por c
    graphVerticalLine('rline', parseFloat(tablaG[x][4]), parseFloat(tablaG[x][5]), parseFloat(tablaG[x][3]))
    
    //linea vertical del 0 al punto f(a)
    graphVerticalLine('gline', parseFloat(tablaG[x][4]), 0, parseFloat(tablaG[x][1]))

    //linea vertcal del 0 al punto f(b)
    graphVerticalLine('tline', 0, parseFloat(tablaG[x][5]), parseFloat(tablaG[x][2]))
    myChart.update()
    
}

function actualizarRFM(x){
    slider.value = parseInt(tablaG[x][0])
    sliderValue.textContent = slider.value;
    //Actualizar labels
    actlblRaiz(tablaG[x][7], tablaG[x][3], tablaG.length-1, x)
    lblerr.textContent = 'Error absoluto raices: ' + tablaG[x][7];
    //Actualizar el punto c
    myChart.data.datasets[1].label = 'c = ' + parseFloat( tablaG[x][3]).toFixed(5);
    myChart.data.datasets[1].data =  [{x:parseFloat(tablaG[x][3]), y : 0}];

    //Actualizar el punto f(a)
    myChart.data.datasets[2].label = '(a): (' + parseFloat( tablaG[x][1]).toFixed(5) + ' , '+ parseFloat( tablaG[x][4]).toFixed(5) + ')';
    myChart.data.datasets[2].data =  [{x:parseFloat(tablaG[x][1]), y : parseFloat(tablaG[x][4])}];

    //Actualizar el punto f(b)
    myChart.data.datasets[3].label = '(b): (' + parseFloat( tablaG[x][2]).toFixed(5) + ' , '+ parseFloat( tablaG[x][5]).toFixed(5) + ')';
    myChart.data.datasets[3].data =  [{x:parseFloat(tablaG[x][2]), y : parseFloat(tablaG[x][5])}];


    myChart.data.datasets[4].data.push(  
        {x:parseFloat(tablaG[x][1]), y : parseFloat(tablaG[x][4])},
        {x:parseFloat(tablaG[x][2]), y : parseFloat(tablaG[x][5])}
    );

    //linea vertical de a a b pasando por c
    graphVerticalLine('rline', parseFloat(tablaG[x][4]), parseFloat(tablaG[x][5]), parseFloat(tablaG[x][3]))
    
    //linea vertical del 0 al punto f(a)
    graphVerticalLine('gline', parseFloat(tablaG[x][4]), 0, parseFloat(tablaG[x][1]))

    //linea vertcal del 0 al punto f(b)
    graphVerticalLine('tline', 0, parseFloat(tablaG[x][5]), parseFloat(tablaG[x][2]))
    myChart.update();
    
    
}




const funciones = {
    a: x => Math.pow(x, 3) + 4 * Math.pow(x, 2) - 10, // x³ + 4x² - 10
    b: x => Math.pow(x, 3) - 2 * Math.pow(x, 2) - 5,  // x³ - 2x² - 5
    c: x => Math.pow(x, 3) + 3 * Math.pow(x, 2) - 1,  // x³ + 3x² - 1
    d: x => x - Math.cos(x),  // x - cos(x)
    e: x => Math.exp(x) + Math.pow(2, -x) + 2 * Math.cos(x) - 6  // e^x + 2^(-x) + 2cos(x) - 6
};

const funcionesFixed =[ 
    {a: x => {      // ( 10-4x²)^1/3
        let det = (10-4*Math.pow(x,2))
        if (det<0){
            return (-1)*Math.pow((-1)*det, (1/3))
        }
        return Math.pow(det, (1/3))
    }, 
    b: x => Math.pow((2*Math.pow(x,2)+5), (1/3) ),  // ( 2x² + 5)^(1/3)
    c: x => {      // (1-3x² )^1/3
        let det = (1-3*Math.pow(x,2))
        if (det<0){
            return (-1)*Math.pow((-1)*det, (1/3))
        }
        return Math.pow(det, (1/3))
    },  
    d: x =>  Math.cos(x),  //  cos(x)
    e: x => Math.exp(x) + Math.pow(2, -x) + 2 * Math.cos(x) - 6 +x }, // e^x + 2^(-x) + 2cos(x) - 6 + x}
    {
        a: x => {      // ( 10-4x²)^1/3
            let det = ((10-Math.pow(x,3))/4)
           
            return Math.pow(det, (1/2))
        }, 
        b: x => {      // ( 10-4x²)^1/3
            let det = ((Math.pow(x,3)-5)/2)
           
            return Math.pow(det, (1/2))
        },   // ( 2x² + 5)^(1/3)
        c: x => {      // (1-3x² )^1/3
            let det = ((1-Math.pow(x,3))/3)
            
            return Math.pow(det, (1/2))
        },  
        d: x => 2*x - Math.cos(x),  //  cos(x)
    },
    { a: x => Math.pow(x, 3) + 4 * Math.pow(x, 2)+x - 10, // x³ + 4x² - 10
        b: x => Math.pow(x, 3) - 2 * Math.pow(x, 2)+x - 5,  // x³ - 2x² - 5
        c: x => Math.pow(x, 3) + 3 * Math.pow(x, 2) +x- 1,  // x³ + 3x² - 1
    }
]

const funcionesDerivadas = {
    a: x => 3 * Math.pow(x,2) + 8 * x,  // 3x2 + 8x
    b: x => 3 * Math.pow(x,2) - 4 * x,  // 3x2 - 4x 
    c: x => 3 * Math.pow(x,2) + 6 * x,  // 3x^2 + 6x 
    d: x =>  1+Math.sin(x),  //   sen(x)
    e: x => Math.exp(x) - Math.log(2) * Math.pow(2, -x) + 2 * Math.cos(x) - 6  // e^x - ln(2) * 2^(-x) - 2cos(x)
}

export function fixed_point(opcion, p,g, tol, i) {
    const func = funciones[opcion];
    console.log(funcionesFixed[g][opcion])
    const gx = funcionesFixed[g][opcion];

    if (!func) {
        console.error("Opción de función no válida.");
        return null;
    }

    let iteraciones = 0;
    let p1;
    let tabla = []; // Almacena las quintuplas de cada iteración

    while (iteraciones < i) {
        p1 = gx(p);

        // Guardar la quintupla (iteración, p, p1, ,g(p), f(p), error)
        tabla.push([iteraciones + 1, p, p1, gx(p), func(p1), Math.abs((p1 - p))]);

        if (p1 === 0 || Math.abs((p1 - p))  < tol || p1 === NaN || p1 === Infinity) break; // Convergencia

        p = p1

        iteraciones++;
    }
    return { raiz: p, tabla };

    return
}

export function bisection(opcion, a, b, tol, i) {
    //hola
    const func = funciones[opcion];

    if (!func) {
        console.error("Opción de función no válida.");
        return null;
    }

    if (func(a) * func(b) > 0) {
        console.error("f(a) y f(b) no tienen signos distintos: " + func(a) + " " + func(b));
        return null;
    }

    let iteraciones = 0;
    let c;
    let tabla = []; // Almacena las quintuplas de cada iteración

    while (iteraciones < i) {
        c = (a + b) / 2;
        let fc = func(c);

        // Guardar la quintupla (iteración, a, b, c,f(a), f(b) f(c), inter)
        tabla.push([iteraciones + 1, a, b, c, func(a), func(b), fc, (b - a) / 2]);

        if (fc === 0 || (b - a) / 2 < tol) break; // Convergencia

        if (func(a) * fc > 0) {
            a = c;
        } else {
            b = c;
        }

        iteraciones++;
    }

    return { raiz: c, tabla };
}


export function false_position(opcion, a, b, tol, i) {
    const func = funciones[opcion];

    if (!func) {
        console.error("Opción de función no válida.");
        return null;
    }

    if (func(a) * func(b) > 0) {
        console.error("f(a) y f(b) no tienen signos distintos");
        return null;
    }

    let iteraciones = 0;
    let c;
    let ant= c;
    let tabla = []; // Almacena las quintuplas de cada iteración

    while (iteraciones < i) {
        c = (b * func(a) - a * func(b)) / (func(a) - func(b));
        let fc = func(c);

        // Guardar la quintupla (iteración, a, b, c,f(a), f(b), f(c), Ea)
        tabla.push([iteraciones + 1, a, b, c, func(a), func(b), fc, Math.abs(c-ant)]);

        if (fc === 0 || Math.abs(fc) < tol) break; // Convergencia

        if (func(a) * fc > 0) {
            a = c;
        } else {
            b = c;
        }
        ant =c
        iteraciones++;
    }

    return { raiz: c, tabla };
}

export function newton_r(opcion, x, tol, i){
    const func = funciones[opcion];
    const der = funcionesDerivadas[opcion];

    let iteraciones = 0;
    let xn;
    
    let tabla = []; 

    while (iteraciones < i) {
        xn = x - func(x)/der(x);

        // Guardar la quintupla (iteración, x, xn, f(xn) f(x), error)
        tabla.push([iteraciones + 1, x, xn, func(xn), func(x), Math.abs(xn-x)]);

        if (func(x) === 0 || Math.abs(xn-x) < tol) break; // Convergencia

        x= xn
        iteraciones++;
        
    }

    return { raiz: xn, tabla };

}

export function secante(opcion, x0, x1, tol, i) {
    const func = funciones[opcion];
    if (!func) {
        console.error("Opción de función no válida.");
        return null;
    }

    let iteraciones = 0;
    let x2;
    let tabla = [];

    while (iteraciones < i) {
        let fx0 = func(x0);
        let fx1 = func(x1);
        
        if (fx1 - fx0 === 0) {
            console.error("División por cero en el método de la secante");
            return null;
        }

        x2 = x1 - (fx1 * (x1 - x0)) / (fx1 - fx0);
        let error = Math.abs(x2 - x1);

        // Guardar los mismos datos que Newton-Raphson
        tabla.push([iteraciones + 1, x1, x2, func(x2), func(x1), error]);

        if (error < tol) break;

        x0 = x1;
        x1 = x2;
        iteraciones++;
    }

    return { raiz: x2, tabla };
}


export function regula_falsi_modificada(opcion, a, b, tol, i) {
    const func = funciones[opcion];

    if (!func) {
        console.error("Opción de función no válida.");
        return null;
    }

    if (func(a) * func(b) > 0) {
        console.error("f(a) y f(b) no tienen signos distintos");
        return null;
    }

    let iteraciones = 0;
    let c, fc;
    let ant = null;
    let tabla = [];

    let fa = func(a), fb = func(b);

    while (iteraciones < i) {
        fa = func(a);
        fb = func(b);
        c = (b * fa - a * fb) / (fa - fb);
        fc = func(c);

        let error = ant !== null ? Math.abs((c - ant) / c) : null;
        ant = c;

        tabla.push([iteraciones + 1, a, b, c, fa, fb, fc, error]);

        if (Math.abs(fc) < tol || (error !== null && error < tol)) break;

        // Regla falsa modificada: si el mismo extremo se mantiene, su f(x) se divide por 2
        if (fa * fc > 0) {
            a = c;
            fa = func(a) / 2; // Ajuste
        } else {
            b = c;
            fb = func(b) / 2; // Ajuste
        }

        iteraciones++;
    }

    return { raiz: c, tabla };
}

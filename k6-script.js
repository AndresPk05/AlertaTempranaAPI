import http from 'k6/http';
import { check, sleep } from 'k6' ;

export const options = {
  vus: 10,           // 10 usuarios virtuales
  iterations: 1000,  // Total de peticiones
  duration: '30s',   // Tiempo total
};

// Índice global para iteraciones (variable compartida entre VUs)
let globalIndex = 0;

// Bloque para incrementar el índice global de forma segura
function getGlobalIndex() {
  return globalIndex++;
}

// Generar placa de vehículo
function generateVehiclePlate() {
  const letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const numbers = '0123456789';
  return `${letters.charAt(Math.floor(Math.random() * letters.length))}${letters.charAt(Math.floor(Math.random() * letters.length))}${letters.charAt(Math.floor(Math.random() * letters.length))}-${numbers.charAt(Math.floor(Math.random() * numbers.length))}${numbers.charAt(Math.floor(Math.random() * numbers.length))}${numbers.charAt(Math.floor(Math.random() * numbers.length))}`;
}

// Generar coordenadas
function generateCoordinates() {
  return {
    latitude: (Math.random() * 180 - 90).toFixed(6),
    longitude: (Math.random() * 360 - 180).toFixed(6),
  };
}

// Generar tipo de mensaje usando índice global calculado
function generateType(globalIndex) {
  return globalIndex < 99 ? 0 : 1;
}

// Función principal
export default function () {
  // Cálculo del índice global
  //const globalIndex = (__VU - 1) * (options.iterations / options.vus) + __ITER;
  //console.log(`Iteración global: ${globalIndex}`);
  const payload = JSON.stringify({
    type: generateType(getGlobalIndex()),
    vehicle_plate: generateVehiclePlate(),
    coordinates: generateCoordinates(),
    status: 0,
  });

  const headers = { 'Content-Type': 'application/json' };
  if(payload.type === 1) {
    console.log(`Mensaje de tipo 1 enviado en la iteración ${globalIndex}`);
    console.log(payload);
  }

  const res = http.post('https://al-70465927e2de468ea37f2932645eb885.ecs.us-east-2.on.aws/event', payload, { headers });

  if(payload.type === 1)
    console.log(JSON.stringify({
      globalIndex,
      type: payload.type,
      timestamp: new Date().toISOString(),
      status: res.status,
      duration: res.timings.duration
    }));



  check(res, {
    'is status 200': (r) => r.status === 200,
  });

  sleep(0.1);
}

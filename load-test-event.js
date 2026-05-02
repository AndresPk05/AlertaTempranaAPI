import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 10,
  duration: '30s',
  iterations: 1000,
  thresholds: {
    http_req_duration: ['avg<500'],
    http_req_failed: ['rate<0.01'],
  },
};

const BASE_URL = 'http://localhost:8080';
const ENDPOINT = '/event';

const params = {
  headers: {
    'Content-Type': 'application/json',
  },
};

function getRandomPlate() {
  const letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const numbers = '0123456789';
  const l1 = letters[Math.floor(Math.random() * letters.length)];
  const l2 = letters[Math.floor(Math.random() * letters.length)];
  const l3 = letters[Math.floor(Math.random() * letters.length)];
  const n1 = numbers[Math.floor(Math.random() * numbers.length)];
  const n2 = numbers[Math.floor(Math.random() * numbers.length)];
  const n3 = numbers[Math.floor(Math.random() * numbers.length)];
  const n4 = numbers[Math.floor(Math.random() * numbers.length)];
  return `${l1}${l2}${l3}-${n1}${n2}${n3}${n4}`;
}

function getPayload(type) {
  return JSON.stringify({
    type: type,
    vehicule_plate: getRandomPlate(),
    coordinates: {
      latitude: 4.710989 + (Math.random() - 0.5) * 0.1,
      longitude: -74.072090 + (Math.random() - 0.5) * 0.1,
    },
    status: 0,
  });
}

export default function () {
  const type = Math.random() < 0.003 ? 2 : 1;
  const payload = getPayload(type);

  const res = http.post(`${BASE_URL}${ENDPOINT}`, payload, params);

  check(res, {
    'status is 200 or 500': (r) => [200, 500].includes(r.status),
    'response has body': (r) => r.body && r.body.length > 0,
  });

  sleep(0.1);
}
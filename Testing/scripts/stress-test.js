import { check, sleep } from 'k6';
import http from 'k6/http';

const baseUrl = __ENV.BASE_TARGET_URL;

export let options = {
    stages: [
        // { iterations: 1, vus: 1}
        { duration: '10s', target: 10 },
		{ duration: '10s', target: 30 },
		{ duration: '60s', target: 50 },
		{ duration: '10s', target: 30 },
		{ duration: '10s', target: 10 },
    ]
};

export default async function () {

    const randMember = Math.floor(Math.random() * 1000) + 1;

    const member = {
        "email": `user${randMember}@example.com`,
        "password": "12345678"
        }
    const url = `${baseUrl}umbraco/surface/login/HandleDirectLogin`;
    const body = JSON.stringify({'email':member.email,'password':member.password});
    const options = {
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*',
            'accept-encoding': 'gzip, deflate, br',
        }
    };

    const loginResult = http.post(url, body, options);
    check(loginResult, {"stress-test: status is 200": (r) => r.status === 200});
    
    sleep(1);
};

import axios from 'axios';

const AxiosInstance = () => {
    const apiURL = "https://localhost:5001";
    const instance = axios.create({
        baseURL: `${apiURL}/api/v1`,
        timeout: 60000
    });

    instance.defaults.headers.get['Pragma'] = 'no-cache';
    instance.defaults.headers.common['Content-Type'] = 'application/json';
    instance.interceptors.response.use(
        response => response.data,
        error => {
            if (error.response) {
                return Promise.reject(error.response.data);
            }
        }
    );

    return instance;
};

let instance = AxiosInstance();

export const API = {
    hotel: {
        search: request => instance.post(`/hotel-search`, request)
    }
};
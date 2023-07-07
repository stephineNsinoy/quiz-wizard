import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/api/tokens`;

const TakersService = {
  acquire: (taker) => axios.post(`${BASE_URL}/acquire`, taker),
  renew: (refreshToken) => axios.post(`${BASE_URL}/renew`, refreshToken),
};

export default TakersService;

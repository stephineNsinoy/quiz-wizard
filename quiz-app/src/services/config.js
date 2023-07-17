import { isLocal } from "../utils/destinations";

let apiUrl = "https://quizwizardapi.azurewebsites.net";

if (isLocal) {
  apiUrl = "https://localhost:7137";
}

const config = {
  API_URL: apiUrl,
};

export default config;

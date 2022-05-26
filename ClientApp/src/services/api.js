import axios from "axios";

class Api {
  getResource = async (url, body, method, headers = {}, data) => {
    let response = null;
    response = await axios({
      url: `api/${url}`,
      method: method,
      data: body,
      params: data,
      headers: {
        "Content-Type": "application/json",
        ...headers,
      },
    });

    return response.data;
  };
  createTODO = async (data) => {
    let url = "todo/createTODO";
    const response = await this.getResource(url, data, "POST", null, null);
    return response;
  };
  deleteTODO = async (id) => {
    let url = `todo/delete/${id}`;
    const response = await this.getResource(url, null, "GET", null, null);
    return response;
  };
  getTodoElement = async () => {
    let url = "todo/getElements";
    const response = await this.getResource(url, null, "GET", null, null);
    return response;
  };
  changeIsDone = async (data) => {
    let url = "todo/changeIsDone";
    const response = await this.getResource(url, data, "POST", null, null);
    return response;
  };
  editTodo = async (data) => {
    let url = "todo/edit";
    const response = await this.getResource(url, data, "POST", null, null);
    return response;
  };
}
export default new Api();
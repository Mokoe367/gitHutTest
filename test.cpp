#include <iostream>
#include<fstream>

using namespace std;

void dijkstra(int** graph, int start, int size) {

    int* dist = new int[size];
    bool* visited = new bool[size];

    for(int i = 0; i < size; i++) {
        dist[i] = INT16_MAX;
        visited[i] = false;
    }

    dist[start] = 0;

    for(int i = 0; i < size - 1; i++) {

        int index;
        int min = INT16_MAX;

        for(int j = 0; j < size; j++) {
            if(visited[j] == false && dist[j] <= min) {
                min = dist[j];
                index = j;
            }
        }

        visited[index] = true;

        for(int j = 0; j < size; j++) {
            if (!visited[j] && graph[index][j] && dist[index] != INT16_MAX && dist[index] + graph[index][j] < dist[j]) {
                dist[j] = dist[index] + graph[index][j];
            }    
        }

    }

    cout << "Vertex : Distance from start" << endl;
    for(int i = 0; i < size; i++) {
        cout << i << " : " << dist[i] << endl;
    }

}


int main() {

    ifstream file1;
    file1.open("input.txt");
  
    int size = 0;   
    int start;                        
    cout << "number of vertices:" << endl;
    cin >> size;
    cout << "input start node:" << endl;
    cin >> start;
    while(start < 0 || start > size) {
        cout << "out of bounds" << endl;
        cout << "input start node:" << endl;
        cin >> start;
    }

    int** graph = new int * [size];

    for(int i = 0; i < size; i++) {
        graph[i] = new int[size];
    }

    for(int i = 0; i < size; i++) {
        for(int j = 0; j < size; j++) {
            file1 >> graph[i][j];
        }
    } 
    file1.close();

    dijkstra(graph, start, size);



    return 0;
}
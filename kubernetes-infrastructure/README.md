# Installation steps

1. Push Dockerfile to Docker hub
   a. Log in to Docker Cloud using the `docker login` command.

   ```sh
   $ docker login
   ```

   b. Tag your image using `docker tag`.

   ```sh
   $ docker tag my_image tourstream/image-server:TAG
   ```

   c. Push your image to Docker Hub using `docker push`.

   ```sh
   $ docker push tourstream/image-server:TAG
   ```

2. Connect to the cluster.

```sh
gcloud container clusters get-credentials CLUSTER_NAME --zone ZONE_NAME --project PROJECT_NAME
```

3. Create the app settings (ConfigMap).

```sh
kubectl craete -f configmap.yaml
```

4. Craete the deployment.

```sh
kubectl craete -f deployment.yaml
```

5. Create the service.

```sh
kubectl craete -f service.yaml
```

6. Craete the load balancer (Ingress).

```sh
kubectl craete -f ingress.yaml
```

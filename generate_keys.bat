@echo off
rem Genera la chiave di crittografia per firmare gli assembly

sn -k 2048 indianapark.snk
sn -p indianapark.snk indianapark_public.snk
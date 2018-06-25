<template>
  <div>
    <v-card class="grid-card">
      <div class="page-title">
        <h2> {{ title }} </h2>

        <v-container >
          <v-card-title>
            <v-text-field v-model="search"
                          append-icon="search"
                          label="Search"
                          single-line
                          hide-details>
            </v-text-field>

            <v-spacer></v-spacer>

            <v-btn class="white--text"
                  slot="activator"
                  @click="addNewItem">
              <v-icon color="white--text">
                      add
              </v-icon>
              {{ $t("common.add") }}
            </v-btn>

          </v-card-title>

          <v-data-table :headers="headers"
                        :items="items">
            <template slot="items" slot-scope="props">

              <td v-for="(header, i) in headers"
                  :key="i">
                <template v-if="!header.isAction">
                  {{ props.item[header.value] }}
                </template>

                <template v-else>
                  <v-btn v-if="showEdit" icon class="mx-0" @click="editItem(props.item)">
                    <v-icon color="#232222">edit</v-icon>
                  </v-btn>

                  <v-btn v-if="showDelete" icon class="mx-0" @click="deleteItem(props.item)">
                    <v-icon color="#232222">delete</v-icon>
                  </v-btn>
                </template>
              </td>
            </template>
          </v-data-table>
        </v-container>
      </div>
    </v-card>
  </div>
</template>

<script>
export default {
  props: {
    items: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },

    headers: {
      type: Array,
      required: false
    },

    title: {
      type: String,
      required: false
    },

    showEdit: {
      type: Boolean,
      required: false
    },

    showDelete: {
      type: Boolean,
      required: false
    }
  },

  data() {
    return {
      search: ""
    };
  },

  methods: {
    addNewItem() {
      const vm = this;

      vm.$emit("newaction");
    },

    editItem(item) {
      const vm = this;

      vm.$emit("action", item);
    },

    deleteItem(item) {
      const vm = this;

      vm.$emit("deleteitem", item);
    }
  }
};
</script>

<style>
.grid-card.card {
  box-shadow: inherit;
  height: 100vh !important;
}
.grid-card .page-title h2 {
  padding-left: 30px;
  padding-top: 20px;
}
.datatable__actions {
  background-color: #f8f8f8 !important;
  padding-top: 20px;
}
.datatable__actions__range-controls {
  margin-right: 15px;
}
.white--text {
  width: 158px;
  margin-right: 0;
  background-color: #ff7107 !important;
}
.table__overflow {
  margin-top: 36px;
}
/* .grid-card {
    min-height: 667px;
    margin-left: 4px;
    margin-right: 4px;
    margin-top: 7px;
    margin-bottom: 14px;
    padding: 30px 20px;
    border-radius: 15px;
    background-color: #fff;
    box-shadow: 0 5px 5px 0 rgba(0,0,0,.24), 0 1px 5px 0 rgba(0,0,0,.12);
  }

  .page-title {
    font-size: 17px;
    line-height: 3.25;
    color: #464646;
  } */

/* .elevation-2 {
    border-radius: 15px
  } */
/* .page-title h1 {
    font-size: 34px;
    line-height: 1.32;
    color: #000;
    margin-bottom: 25px;
  } */

table.table thead tr {
  height: 70px;
}

table.table thead th {
  font-weight: bold;
  font-size: initial;
}

table.table thead td {
  font-weight: 400;
  font-size: 75%;
}

table.table tbody td:last-child {
  text-align: right;
}
tr:nth-child(even) {
  background-color: #f2f2f2;
}
</style>

<template>
  <div>
    <v-card class="grid-card">
      <div class="page-title">
        {{ title }}
      </div>

      <v-container class="elevation-3">
        <v-card-title>
          <v-btn color="teal"
                 class="white--text"
                 round
                 slot="activator"
                 @click="addNewItem">
            <v-icon color="white--text">
                    add
            </v-icon>
            {{ $t("comman.add") }}
          </v-btn>

          <v-spacer></v-spacer>

          <v-text-field v-model="search"
                        prepend-icon="search"
                        label="Search"
                        single-line
                        hide-details>
          </v-text-field>
        </v-card-title>

        <v-data-table :headers="headers"
                      :items="items">
          <template slot="items" slot-scope="props">
            <td v-for="(header, i) in headers" :key="i">
              {{ props.item[header.value] }}
            </td>

            <td class="justify-center layout px-0" slot="activator">
              <v-btn v-if="showEdit" icon class="mx-0" @click="editItem(props.item)">
                <v-icon color="teal">edit</v-icon>
              </v-btn>

              <v-btn v-if="showDelete" icon class="mx-0" @click="deleteItem(props.item)">
                <v-icon color="pink">delete</v-icon>
              </v-btn>
            </td>

            <template slot="no-data">
              <v-btn color="primary">Reset</v-btn>
            </template>
          </template>
        </v-data-table>
      </v-container>
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
      search: ''
    };
  },

  methods: {
    addNewItem() {
      const vm = this;

      vm.$emit('newaction');
    },

    editItem(item) {
      const vm = this;

      vm.$emit('action', item);
    },

    deleteItem(item) {
      const vm = this;

      vm.$emit('deleteitem', item);
    }
  }
};

</script>

<style>
  .grid-card {
    margin: 15px;
    padding: 50px;
    border-radius: 15px;
    box-shadow: 0 5px 5px 0 rgba(0, 0, 0, 0.24), 0 1px 5px 0 rgba(0, 0, 0, 0.12)
  }

  .page-title {
    font-size: 20px;
    line-height: 2.25;
    color: #464646;
  }
</style>
